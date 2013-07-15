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
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cocos2D;

namespace Cocos3D
{
    public class LCC3ProgPipeline : ICC3VertexTypeDataSource
    {
        // Static fields

        public const int VertexAttributeIndexUnavailable = -1;

        private static LCC3ProgPipeline _sharedProgPipeline;

        // Instance fields

        private GraphicsDevice _xnaGraphicsDevice;
        private VertexBuffer _xnaVertexBuffer;
        private BlendState _xnaBlendState;
        private CullMode _xnaCullMode;
        private DepthStencilState _xnaDepthStencilState;
        private float _xnaDepthBias;
        private float _xnaSlopeScaleDepthBias;

        private Color _xnaClearColor;
        private float _xnaClearDepth;
        private int _xnaClearStencil;

        private Texture2D[] _xnaTextureUnits;
        private int _xnaCurrentlyActiveTextureUnitIndex;

        private List<LCC3VertexAttr> _vertexAttributes;
        private CC3VertexType[] _vertexData;

        private Stack<LCC3Matrix4x4> _modelMatrixStack;
        private Stack<LCC3Matrix4x4> _viewMatrixStack;
        private Stack<LCC3Matrix4x4> _projMatrixStack;


        #region Allocation and initialization

        public static LCC3ProgPipeline SharedPipeline()
        {
            if (_sharedProgPipeline == null)
            {
                _sharedProgPipeline = new LCC3ProgPipeline(null);
            }

            return _sharedProgPipeline;
        }

        public LCC3ProgPipeline(GraphicsDevice xnaGraphicsDevice)
        {
            CC3VertexType.DataSource = this;

            _xnaGraphicsDevice = xnaGraphicsDevice;
            _xnaBlendState = BlendState.Opaque;
            _xnaCullMode = CullMode.None;
            _xnaDepthStencilState = DepthStencilState.None;
            _xnaDepthBias = 0.0f;
            _xnaSlopeScaleDepthBias = 0.0f;

            _modelMatrixStack = new Stack<LCC3Matrix4x4>();
            _viewMatrixStack = new Stack<LCC3Matrix4x4>();
            _projMatrixStack = new Stack<LCC3Matrix4x4>();

            this.InitVertexAttributes();
            this.InitTextureUnits();
        }

        public void InitVertexAttributes()
        {
            _vertexAttributes = new List<LCC3VertexAttr>();
        }

        public void InitTextureUnits()
        {
            _xnaTextureUnits = new Texture2D[(int)LCC3ProgPipeline.MaxNumberOfTextureUnits()];
            _xnaCurrentlyActiveTextureUnitIndex = 0;
        }

        #endregion Allocation and initialization


        #region ICC3VertexTypeDataSource methods

        public bool VertexPositionEnabled()
        {
            return _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribPosition].WasBound;
        }

        public bool VertexTexCoordEnabled()
        {
            return _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribTexCoords].WasBound;
        }

        public bool VertexColorEnabled()
        {
            return _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribColor].WasBound;
        }
       
        #endregion ICC3VertexTypeDataSource


        #region Capabilities

        public void EnableBlend(bool onOrOff)
        {
            if (onOrOff == true)
            {
                _xnaGraphicsDevice.BlendState = _xnaBlendState;
            } 
            else
            {
                _xnaGraphicsDevice.BlendState = BlendState.Opaque;
            }
        }

        public void EnableCullFace(bool onOrOff)
        {
            if (onOrOff == true)
            {
                _xnaGraphicsDevice.RasterizerState.CullMode = _xnaCullMode;
            } 
            else
            {
                _xnaGraphicsDevice.RasterizerState.CullMode = CullMode.None;
            }
        }

        public void EnableDepthTest(bool onOrOff)
        {
            _xnaDepthStencilState.DepthBufferEnable = onOrOff;
            _xnaGraphicsDevice.DepthStencilState = _xnaDepthStencilState;
        }

        public void EnablePolygonOffset(bool onOrOff)
        {
            if (onOrOff == true)
            {
                _xnaGraphicsDevice.RasterizerState.DepthBias = _xnaDepthBias;
                _xnaGraphicsDevice.RasterizerState.SlopeScaleDepthBias = _xnaSlopeScaleDepthBias;
            } 
            else
            {
                _xnaGraphicsDevice.RasterizerState.DepthBias = 0.0f;
                _xnaGraphicsDevice.RasterizerState.SlopeScaleDepthBias = 0.0f;
            }
        }

        public void EnableScissorTest(bool onOrOff)
        {
            _xnaGraphicsDevice.RasterizerState.ScissorTestEnable = onOrOff;
        }

        public void EnableStencilTest(bool onOrOff)
        {
            _xnaDepthStencilState.StencilEnable = onOrOff;
            _xnaGraphicsDevice.DepthStencilState = _xnaDepthStencilState;
        }

        #endregion Capabilities


        #region Vertex attributes

        public void BindMeshWithVisitor(LCC3Mesh mesh, 
                                        LCC3NodeDrawingVisitor visitor)
        {
            mesh.VertexIndices.BindContentToAttributeAtIndexWithVisitor(LCC3ProgPipeline.VertexAttributeIndexUnavailable, visitor);
        }

        public void BindVertexAttributeWithVisitor(LCC3ShaderAttribute attribute, 
                                                   LCC3NodeDrawingVisitor visitor)
        {
            LCC3VertexArray vertexArray = this.VertexArrayForAttributeWithVisitor(attribute, visitor);
            vertexArray.BindContentToAttributeAtIndexWithVisitor(attribute.Location, visitor);
        }

        public void EnableVertexAttributeAtIndex(bool onOrOff, int vaIndex)
        {
            if (vaIndex >= 0)
            {
                LCC3VertexAttr vertexAttribute = _vertexAttributes[vaIndex];

                vertexAttribute.IsEnabled = onOrOff;
                vertexAttribute.IsEnabledKnown = true;
            }
        }

        public LCC3VertexArray VertexArrayForAttributeWithVisitor(LCC3ShaderAttribute attribute, 
                                                                  LCC3NodeDrawingVisitor visitor)
        {
            return visitor.CurrentMesh.VertexArrayForSemanticAtIndex(attribute.SemanticVertex, attribute.SemanticVertexIndex);
        }


        public void BindVertexContentToAttributeAtIndex(object[] pData, 
                                                        uint elemSize, 
                                                        LCC3VertexAttrElementType elemType, 
                                                        uint vertexStride, 
                                                        bool shouldNormalize, 
                                                        int vaIndex)
        {
            if (vaIndex >= 0)
            {
                LCC3VertexAttr vertexAttribute = _vertexAttributes[vaIndex];

                vertexAttribute.Vertices = pData;
                vertexAttribute.ElementSize = elemSize;
                vertexAttribute.ElementType = elemType;
                vertexAttribute.VertexStride = vertexStride;
                vertexAttribute.ShouldNormalize = shouldNormalize;
                vertexAttribute.WasBound = true;
            }
        }

        public void ClearUnboundVertexAttributes()
        {
            foreach (LCC3VertexAttr vertexAttribute in _vertexAttributes)
            {
                vertexAttribute.WasBound = false;
            }
        }

        public void EnableBoundVertexAttributes()
        {
            foreach (LCC3VertexAttr vertexAttribute in _vertexAttributes)
            {
                this.EnableVertexAttributeAtIndex(vertexAttribute.WasBound, _vertexAttributes.IndexOf(vertexAttribute));
            }
        }

        public void Enable2DVertexAttributes()
        {
            foreach (LCC3VertexAttr vertexAttribute in _vertexAttributes)
            {
                this.EnableVertexAttributeAtIndex(false, _vertexAttributes.IndexOf(vertexAttribute));
            }

            _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribPosition].WasBound = true;
            _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribColor].WasBound = true;
            _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribTexCoords].WasBound = true;
        }

        public void GenerateVertexBuffer()
        {
            int numOfVertices = _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribPosition].Vertices.Length;
            _vertexData = new CC3VertexType[numOfVertices];

            for (int i=0; i < numOfVertices; i++)
            {
                LCC3Vector position = (LCC3Vector)_vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribPosition].Vertices[i];
                CCPoint texCoord = (CCPoint)_vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribTexCoords].Vertices[i];
                LCC3Vector4 color = (LCC3Vector4)_vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribColor].Vertices[i];

                _vertexData[i] = new CC3VertexType(position, texCoord, color);
            }

            _xnaVertexBuffer = new VertexBuffer(_xnaGraphicsDevice, 
                                                typeof(CC3VertexType), 
                                                _vertexData.Length, 
                                                BufferUsage.None);

            _xnaVertexBuffer.SetData<CC3VertexType>( _vertexData );
        }

        public void BindVertexBuffer()
        {
            _xnaGraphicsDevice.SetVertexBuffer(_xnaVertexBuffer);
        }

        public void DrawVertices(LCC3DrawMode drawMode, int startIndex, int length)
        {
            _xnaGraphicsDevice.DrawPrimitives(drawMode.XnaPrimitiveType(), startIndex, length);
        }

        #endregion Vertex attributes


        #region State

        public void SetClearColor(CCColor4F color)
        {
            _xnaClearColor = color.XnaColor();
        }

        public void SetClearDepth(float val)
        {
            _xnaClearDepth = val;
        }

        public void SetClearStencil(int val)
        {
            _xnaClearStencil = val;
        }

        public void SetCullFace(LCC3CullMode cullMode)
        {
            _xnaCullMode = cullMode.XnaCullMode();
            this.EnableCullFace(true);
        }

        public void SetDepthFunc(LCC3DepthStencilFuncMode depthFuncMode)
        {
            _xnaDepthStencilState.DepthBufferFunction = depthFuncMode.XnaCompareFunc();
            this.EnableDepthTest(true);
        }

        public void SetDepthMask(bool writable)
        {
            _xnaDepthStencilState.DepthBufferWriteEnable = writable;
            this.EnableDepthTest(true);
        }

        public void SetScissor(LCC3Viewport viewport)
        {
            _xnaGraphicsDevice.ScissorRectangle = viewport.ToCCRect().XnaRect();
            this.EnableScissorTest(true);
        }

        public void SetStencilFunc(LCC3DepthStencilFuncMode stencilFuncMode, int reference, uint mask)
        {
            _xnaDepthStencilState.StencilFunction = stencilFuncMode.XnaCompareFunc();
            _xnaDepthStencilState.ReferenceStencil = reference;
            _xnaDepthStencilState.StencilMask = (int)mask;
            this.EnableStencilTest(true);
        }

        public void SetStencilMask(uint mask)
        {
            _xnaDepthStencilState.StencilMask = (int)mask;
            this.EnableStencilTest(true);
        }

        public void SetOpOnStencilFail(LCC3StencilOperation stencilFailOp, 
                                       LCC3StencilOperation stencilPassDepthFailOp, 
                                       LCC3StencilOperation stencilAndDepthPass)
        {
            _xnaDepthStencilState.StencilFail = stencilFailOp.XnaStencilOperation();
            _xnaDepthStencilState.StencilDepthBufferFail = stencilPassDepthFailOp.XnaStencilOperation();
            _xnaDepthStencilState.StencilPass = stencilAndDepthPass.XnaStencilOperation();
            this.EnableStencilTest(true);
        }

        public void SetViewport(LCC3Viewport viewport)
        {
            _xnaGraphicsDevice.Viewport = viewport.XnaViewport;
        }

        public void ClearBuffers(LCC3BufferMask bufferMask)
        {
            _xnaGraphicsDevice.Clear(bufferMask.XnaBufferMask(), _xnaClearColor, _xnaClearDepth, _xnaClearStencil);
        }

        public void ClearColorBuffer()
        {
            this.ClearBuffers(LCC3BufferMask.ColorBuffer);
        }

        public void ClearDepthBuffer()
        {
            this.ClearBuffers(LCC3BufferMask.DepthBuffer);
        }

        public void ClearStencilBuffer()
        {
            this.ClearBuffers(LCC3BufferMask.StencilBuffer);
        }

        #endregion State


        #region Materials

        public void SetBlendFuncSrcAndDst(uint src, uint dst)
        {

        }

        #endregion Materials


        #region Textures

        public void LoadTextureImage<T>(T[] imageData, 
                                        int width, 
                                        int height,
                                        LCC3TextureFormat textureFormat,
                                        uint texUnitIndex) where T : struct
        {
            this.ActivateTextureUnit(texUnitIndex);

            _xnaTextureUnits[_xnaCurrentlyActiveTextureUnitIndex] 
                = new Texture2D(_xnaGraphicsDevice, width, height, false, textureFormat.XnaSurfaceFormat());

            _xnaTextureUnits[_xnaCurrentlyActiveTextureUnitIndex].SetData<T>(imageData);
        }
       
        public void ActivateTextureUnit(uint texUnitIndex)
        {
            _xnaCurrentlyActiveTextureUnitIndex = (int)texUnitIndex;
        }

        #endregion Textures


        #region Matrices

        public void PushModelMatrixStack(LCC3Matrix4x4 matrix)
        {
            _modelMatrixStack.Push(matrix);
        }

        public void PopModelMatrixStack()
        {
            _modelMatrixStack.Pop();
        }

        public void PushViewMatrixStack(LCC3Matrix4x4 matrix)
        {
            _viewMatrixStack.Push(matrix);
        }

        public void PopViewMatrixStack()
        {
            _viewMatrixStack.Pop();
        }

        public void PushProjectionMatrixStack(LCC3Matrix4x4 matrix)
        {
            _projMatrixStack.Push(matrix);
        }

        public void PopProjectionMatrixStack()
        {
            _projMatrixStack.Pop();
        }

        #endregion Matrices


        #region Platform limits

        public static uint MaxNumberOfLights()
        {
            return 8;
        }

        public static uint MaxNumberOfClipPlanes()
        {
            return 6;
        }

        public static uint MaxNumberOfPaletteMatrices()
        {
            return 12;
        }

        public static uint MaxNumberOfTextureUnits()
        {
            return 5;
        }

        public static uint MaxNumberOfVertexAttributes()
        {
            return 4;
        }

        public static uint MaxNumberOfVertexUnits()
        {
            return 4;
        }

        #endregion Platform limits


        #region Shaders

        public void BindProgramWithVisitor(LCC3ShaderProgram program, LCC3NodeDrawingVisitor visitor)
        {
            program.BindWithVisitor(visitor);

            this.ClearUnboundVertexAttributes();
            program.PopulateVertexAttributesWithVisitor(visitor);
            this.EnableBoundVertexAttributes();

            program.PopulateNodeScopeUniformsWithVisitor(visitor);
        }

        public string DefaultShaderPreamble()
        {
            return "";
        }

        #endregion Shaders


        #region Aligning 2D & 3D caches

        public void Align2DStateCache()
        {

        }

        public void Align3DStateCache()
        {
            foreach (LCC3VertexAttr vertexAttribute in _vertexAttributes)
            {
                vertexAttribute.IsEnabledKnown = false;
                vertexAttribute.IsKnown = false;
            }
        }

        #endregion Aligning 2D & 3D caches

    }

    #region Private custom vertex declaration

    internal interface ICC3VertexTypeDataSource
    {
        bool VertexPositionEnabled();
        bool VertexTexCoordEnabled();
        bool VertexColorEnabled();
    }

    internal struct CC3VertexType : IVertexType
    {
        private static ICC3VertexTypeDataSource _dataSource;

        private Vector3 _position;
        private Vector2 _texCoord;
        private Vector4 _color;

        public static ICC3VertexTypeDataSource DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        public VertexDeclaration VertexDeclaration 
        { 
            get
            {
                int currentOffset = 0;

                List<VertexElement> vertexElements = new List<VertexElement>();

                if (_dataSource.VertexPositionEnabled())
                {
                    vertexElements.Add(new VertexElement(currentOffset, VertexElementFormat.Vector3, VertexElementUsage.Position, 0));
                    currentOffset += Marshal.SizeOf(Vector3.Zero);
                }

                if (_dataSource.VertexTexCoordEnabled())
                {
                    vertexElements.Add(new VertexElement(currentOffset, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));
                    currentOffset += Marshal.SizeOf(Vector2.Zero);
                }

                if (_dataSource.VertexColorEnabled())
                {
                    vertexElements.Add(new VertexElement(currentOffset, VertexElementFormat.Vector4, VertexElementUsage.Color, 0));
                    currentOffset += Marshal.SizeOf(Vector4.Zero);
                }

                VertexDeclaration vertexDec = new VertexDeclaration(vertexElements.ToArray());

                return vertexDec;
            }
        }

        public CC3VertexType(LCC3Vector position, CCPoint texCoord, LCC3Vector4 color)
        {
            _position = position.XnaVector;
            _texCoord = new Vector2(texCoord.X, texCoord.Y);
            _color = color.XnaVector4;
        }

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 TextureCoordinate
        {
            get { return _texCoord; }
            set { _texCoord = value; }
        }

        public Vector4 Color
        {
            get { return _color; }
            set { _color = value; }
        }
    }

    #endregion Private custom vertex declaration
}

