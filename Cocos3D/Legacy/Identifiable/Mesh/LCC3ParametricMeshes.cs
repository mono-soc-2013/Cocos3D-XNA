//
// Copyright 2013 Rami Tabbara
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//
// Please see README.md to locate the external API documentation.
//
using System;
using Cocos2D;

namespace Cocos3D
{
    public class LCC3ParametricMeshes : LCC3Mesh
    {
        #region Allocation and initialization

        public LCC3ParametricMeshes(int tag, string name) : base(tag, name)
        {

        }

        #endregion Allocation and initialization


        #region Utility methods

        private void EnsureVertexContent()
        {
            if (this.VertexContentTypes == LCC3VertexContent.VertexContentNone)
            {
                this.VertexContentTypes = (LCC3VertexContent.VertexContentLocation | 
                                           LCC3VertexContent.VertexContentNormal | 
                                           LCC3VertexContent.VertexContentTextureCoordinates);
            }
        }

        #endregion Utility methods


        #region Populating parametric plane

        public void PopulateAsRectangle(CCSize rectSize)
        {
            this.PopulateAsRectangle(rectSize, new CCPoint(0.5f, 0.5f));
        }

        public void PopulateAsRectangle(CCSize rectSize, CCPoint origin)
        {
            this.PopulateAsRectangle(rectSize, origin, new LCC3Tessellation(1, 1));
        }

        public void PopulateAsRectangle(CCSize rectSize, CCPoint origin, LCC3Tessellation divsPerAxis)
        {
            divsPerAxis.X = Math.Max(divsPerAxis.X, 1);
            divsPerAxis.Y = Math.Max(divsPerAxis.Y, 1);

            CCPoint rectExtent = new CCPoint(rectSize.Width, rectSize.Height);
            origin = new CCPoint(rectExtent.X * origin.X, rectExtent.Y * origin.Y);
            CCPoint botLeft = CCPoint.Zero - origin;
            CCPoint topRight = rectExtent - origin;

            CCSize divSize = new CCSize((topRight.X - botLeft.X) / divsPerAxis.X, 
                                        (topRight.Y - botLeft.Y) / divsPerAxis.Y);
            CCSize divTexSpan = new CCSize((1.0f / divsPerAxis.X), (1.0f / divsPerAxis.Y));

            LCC3Tessellation verticesPerAxis = new LCC3Tessellation(0,0);
            verticesPerAxis.X = divsPerAxis.X + 1;
            verticesPerAxis.Y = divsPerAxis.Y + 1;
            uint vertexCount = verticesPerAxis.X * verticesPerAxis.Y;
            uint triangleCount = divsPerAxis.X * divsPerAxis.Y * 2;

            this.EnsureVertexContent();
            this.AllocatedVertexCapacity = vertexCount;
            this.AllocatedVertexIndexCapacity = (triangleCount * 3);

            for (uint iy = 0; iy < verticesPerAxis.Y; iy++) 
            {
                for (uint ix = 0; ix < verticesPerAxis.X; ix++) 
                {
                    uint vIndx = iy * verticesPerAxis.X + ix;

                    float vx = botLeft.X + (divSize.Width * ix);
                    float vy = botLeft.Y + (divSize.Height * iy);
                    this.SetVertexLocationAtIndex(new LCC3Vector(vx, vy, 0.0f), vIndx);

                    this.SetNormalAtIndex(LCC3Vector.CC3UnitZPositive, vIndx);

                    float u = divTexSpan.Width * ix;
                    float v = divTexSpan.Height * iy;
                    this.SetVertexTexCoord2FAtIndex(new CCTex2F(u, (1.0f - v)), vIndx);
                }
            }

            LCC3VertexIndices indices = this.VertexIndices;
            uint iIndx = 0;
            for (uint iy = 0; iy < divsPerAxis.Y; iy++) 
            {
                for (uint ix = 0; ix < divsPerAxis.X; ix++) 
                {
                    uint botLeftOfFace;

                    // First triangle of face wound counter-clockwise
                    botLeftOfFace = iy * verticesPerAxis.X + ix;
                    indices.SetIndex(botLeftOfFace, iIndx++);
                    indices.SetIndex(botLeftOfFace + 1, iIndx++);
                    indices.SetIndex(botLeftOfFace + verticesPerAxis.X + 1, iIndx++);

                    // Second triangle of face wound counter-clockwise
                    indices.SetIndex(botLeftOfFace + verticesPerAxis.X + 1, iIndx++);
                    indices.SetIndex(botLeftOfFace + verticesPerAxis.X, iIndx++);
                    indices.SetIndex(botLeftOfFace, iIndx++);
                }
            }
        }

        #endregion Populating parametric plane


        #region Populating parametric sphere

        public void PopulateAsSphere(float radius, LCC3Tessellation divsPerAxis)
        {
            divsPerAxis.X = Math.Max(divsPerAxis.X, 3);
            divsPerAxis.Y = Math.Max(divsPerAxis.Y, 2);

            CCSize divSpan = new CCSize((float)((Math.PI * 2) / divsPerAxis.X), (float)(Math.PI / divsPerAxis.Y));
            CCSize divTexSpan = new CCSize((float)(1.0 / divsPerAxis.X), (float)(1.0 / divsPerAxis.Y));
            float halfDivTexSpanWidth = divTexSpan.Width * 0.5f;

            LCC3Tessellation verticesPerAxis = new LCC3Tessellation(0,0);
            verticesPerAxis.X = divsPerAxis.X + 1;
            verticesPerAxis.Y = divsPerAxis.Y + 1;
            uint vertexCount = verticesPerAxis.X * verticesPerAxis.Y;
            uint triangleCount = divsPerAxis.X * (divsPerAxis.Y - 1) * 2;

            this.EnsureVertexContent();
            this.AllocatedVertexCapacity = vertexCount;
            this.AllocatedVertexIndexCapacity = (triangleCount * 3);
            LCC3VertexIndices indices = this.VertexIndices;

            uint vIndx = 0;         
            uint iIndx = 0;  

            for (uint iy = 0; iy < verticesPerAxis.Y; iy++) 
            {
                float y = divSpan.Height * iy;
                float sy = (float)Math.Sin(y);
                float cy = (float)Math.Cos(y);

                for (uint ix = 0; ix < verticesPerAxis.X; ix++) 
                {
                    float x = divSpan.Width * ix;
                    float sx = (float)Math.Sin(x);
                    float cx = (float)Math.Cos(x);

                    LCC3Vector unitRadial = new LCC3Vector(-(sy * sx), cy, -(sy * cx));
                    this.SetVertexLocationAtIndex(unitRadial * radius, vIndx);

                    this.SetNormalAtIndex(unitRadial.NormalizedVector(), vIndx);

                    float uOffset = 0.0f;
                    if (iy == 0)
                    {
                        uOffset = halfDivTexSpanWidth;
                    }       

                    if (iy == (verticesPerAxis.Y - 1))
                    {
                        uOffset = -halfDivTexSpanWidth;
                    }

                    float u = divTexSpan.Width * ix + uOffset;
                    float v = divTexSpan.Height * iy;
                    this.SetVertexTexCoord2FAtIndex(new CCTex2F(u, v), vIndx);


                    if (iy > 0 && ix > 0) 
                    {
                        if (iy > 1) 
                        {
                            indices.SetIndex(vIndx, iIndx++);
                            indices.SetIndex(vIndx - verticesPerAxis.X, iIndx++);
                            indices.SetIndex(vIndx - verticesPerAxis.Y - 1, iIndx++);
                        }               

                        if (iy < (verticesPerAxis.Y - 1)) 
                        {
                            indices.SetIndex(vIndx - verticesPerAxis.X - 1, iIndx++);
                            indices.SetIndex(vIndx - 1, iIndx++);
                            indices.SetIndex(vIndx, iIndx++);                           
                        }
                    }

                    vIndx++;    
                }
            }
        }

        #endregion Populating parametric sphere


        #region Populating parametric cone

        public void PopulateAsHollowCone(float radius, float height, LCC3Tessellation angleAndHeightDivs)
        {
            uint numAngularDivs = Math.Max(angleAndHeightDivs.X, 3);
            uint numHeightDivs = Math.Max(angleAndHeightDivs.Y, 1);

            float radiusHeightRatio = radius / height;
            float angularDivSpan = (float)(Math.PI * 2) / numAngularDivs;     
            float heightDivSpan = height / numHeightDivs;          
            float radialDivSpan = radius / numHeightDivs;        
            float texAngularDivSpan = 1.0f / numAngularDivs;  
            float texHeightDivSpan = 1.0f / numHeightDivs;

            uint vertexCount = (numAngularDivs + 1) * (numHeightDivs + 1);
            uint triangleCount = 2 * numAngularDivs * numHeightDivs - numAngularDivs;

            this.EnsureVertexContent();
            this.AllocatedVertexCapacity = vertexCount;
            this.AllocatedVertexIndexCapacity = (triangleCount * 3);

            uint vIdx = 0;            
            uint iIdx = 0;

            for (uint ia = 0; ia <= numAngularDivs; ia++) 
            {
                float angle = angularDivSpan * ia;
                float ca = (float)-Math.Cos(angle);      
                float sa = (float)-Math.Sin(angle);

                LCC3Vector vtxNormal = new LCC3Vector(sa, radiusHeightRatio, ca).NormalizedVector();

                for (uint ih = 0; ih <= numHeightDivs; ih++, vIdx++) 
                {
                    float vtxRadius = radius - (radialDivSpan * ih);
                    float vtxHt = heightDivSpan * ih;
                    LCC3Vector vtxLoc = new LCC3Vector(vtxRadius * sa, vtxHt, vtxRadius * ca);

                    this.SetVertexLocationAtIndex(vtxLoc,vIdx);
                    this.SetNormalAtIndex(vtxNormal, vIdx);

                    CCTex2F texCoord = new CCTex2F(texAngularDivSpan * ia, (1.0f - texHeightDivSpan * ih));
                    this.SetVertexTexCoord2FAtIndex(texCoord, vIdx);

                    if (ia < numAngularDivs && ih < numHeightDivs) 
                    {
                        this.SetVertexIndexAtIndex( vIdx, iIdx++);                         
                        this.SetVertexIndexAtIndex((vIdx + numHeightDivs + 1),iIdx++);   
                        this.SetVertexIndexAtIndex((vIdx + numHeightDivs + 2),iIdx++);   

                        if (ih < numHeightDivs - 1) 
                        {
                            this.SetVertexIndexAtIndex((vIdx + numHeightDivs + 2), iIdx++); 
                            this.SetVertexIndexAtIndex((vIdx + 1), iIdx++);                
                            this.SetVertexIndexAtIndex(vIdx, iIdx++);
                        }
                    }
                }
            }
        }

        #endregion Populating parametric cone


        #region Populating parametric boxes

        public void PopulateAsSolidBox(LCC3BoundingBox box, bool useSoftEdges=true)
        {
            float w = box.Maximum.X - box.Minimum.X;      
            float h = box.Maximum.Y - box.Minimum.Y;      
            float d = box.Maximum.Z - box.Minimum.Z;      
            float ufw = d + w + d + w;                    
            float ufh = d + h + d;                        

            this.PopulateAsSolidBox( box, new CCPoint((d / ufw), (d / ufh)));
        }

        private void PopulateAsSolidBox(LCC3BoundingBox box, CCPoint corner, bool useSoftEdges=true)
        {
            LCC3Vector boxMin = box.Minimum;
            LCC3Vector boxMax = box.Maximum;
            uint vertexCount = 24;
            uint triangleCount = 12;

            this.EnsureVertexContent();
            this.AllocatedVertexCapacity = vertexCount;
            this.AllocatedVertexIndexCapacity = (triangleCount * 3);
            LCC3VertexIndices indices = this.VertexIndices;

            // Front face, CCW winding:
            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMin.Y, boxMax.Z), 0);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitZPositive, 0);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(corner.X, (1.0f - corner.Y)), 0);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMin.Y, boxMax.Z), 1);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitZPositive, 1);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.5f, (1.0f - corner.Y)), 1);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMax.Y, boxMax.Z), 2);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitZPositive, 2);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.5f, corner.Y), 2);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMax.Y, boxMax.Z), 3);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitZPositive, 3);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(corner.X, corner.Y), 3);

            // Right face, CCW winding:
            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMin.Y, boxMax.Z), 4);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitXPositive, 4);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.5f, (1.0f - corner.Y)), 4);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMin.Y, boxMin.Z), 5);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitXPositive, 5);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F((0.5f + corner.X), (1.0f - corner.Y)), 5);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMax.Y, boxMin.Z), 6);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitXPositive, 6);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F((0.5f + corner.X), corner.Y), 6);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMax.Y, boxMax.Z), 7);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitXPositive, 7);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.5f, corner.Y), 7);

            // Back face, CCW winding:
            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMin.Y, boxMin.Z), 8);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitZNegative, 8);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F((0.5f + corner.X), (1.0f - corner.Y)), 8);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMin.Y, boxMin.Z), 9);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitZNegative, 9);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(1.0f, (1.0f - corner.Y)), 9);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMax.Y, boxMin.Z), 10);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitZNegative, 10);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(1.0f, corner.Y), 10);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMax.Y, boxMin.Z), 11);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitZNegative, 11);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F((0.5f + corner.X), corner.Y), 11);

            // Left face, CCW winding:
            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMin.Y, boxMin.Z), 12);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitXNegative, 12);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.0f, (1.0f - corner.Y)), 12);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMin.Y, boxMax.Z), 13);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitXNegative, 13);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(corner.X, (1.0f - corner.Y)), 13);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMax.Y, boxMax.Z),14);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitXNegative, 14);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(corner.X, corner.Y), 14);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMax.Y, boxMin.Z), 15);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitXNegative, 15);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.0f, corner.Y), 15);

            // Top face, CCW winding:
            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMax.Y, boxMin.Z), 16);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitYPositive, 16);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(corner.X, 0.0f), 16);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMax.Y, boxMax.Z), 17);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitYPositive, 17);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(corner.X, corner.Y), 17);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMax.Y, boxMax.Z), 18);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitYPositive, 18);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.5f, corner.Y), 18);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMax.Y, boxMin.Z), 19);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYPositive + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitYPositive, 19);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.5f, 0.0f), 19);

            // Bottom face, CCW winding:
            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMin.Y, boxMax.Z), 20);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitYNegative, 20);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(corner.X, (1.0f - corner.Y)), 20);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMin.X, boxMin.Y, boxMin.Z), 21);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXNegative + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitYNegative, 21);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(corner.X, 1.0f), 21);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMin.Y, boxMin.Z), 22);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZNegative) / 3 : LCC3Vector.CC3UnitYNegative, 22);
            this.SetVertexTexCoord2FAtIndex( new CCTex2F(0.5f, 1.0f), 22);

            this.SetVertexLocationAtIndex(new LCC3Vector(boxMax.X, boxMin.Y, boxMax.Z), 23);
            this.SetNormalAtIndex(useSoftEdges ? (LCC3Vector.CC3UnitXPositive + LCC3Vector.CC3UnitYNegative + LCC3Vector.CC3UnitZPositive) / 3 : LCC3Vector.CC3UnitYNegative, 23);
            this.SetVertexTexCoord2FAtIndex(new CCTex2F(0.5f, (1.0f - corner.Y)), 23);

            uint indxIndx = 0;
            uint vtxIndx = 0;
            for (int side = 0; side < 6; side++) 
            {
                // First trangle of side - CCW from bottom left
                indices.SetIndex(vtxIndx++, indxIndx++);
                indices.SetIndex(vtxIndx++, indxIndx++);
                indices.SetIndex(vtxIndx, indxIndx++);

                // Second triangle of side - CCW from bottom left
                indices.SetIndex(vtxIndx++, indxIndx++);
                indices.SetIndex(vtxIndx++, indxIndx++);
                indices.SetIndex(vtxIndx - 4, indxIndx++);
            }
        }

        #endregion Populating parametric boxes
    }
}

