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
using Cocos3D;
using Cocos2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Cocos3DShowcase
{
    public class ShowcaseGameScene : CC3Scene, ILCC3ShaderSemanticDelegate
    {
        // Instance fields

        private LCC3ProgPipeline _progPipeline;
        private CC3CameraPerspective _camera;

        private BasicEffect _basicEffect;
        private List<Matrix> _listOfCubeWorldMatrices;

        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        private Model _tankModel;
        private LCC3GraphicsTexture2D _tankTexture;
        private LCC3ShaderProgram _tankShader;
        private Matrix[] _tankTransforms;

        private const int _numOfMaterialCubes = 4;
        private List<LCC3Matrix4x4> _listOfMaterialCubeMatrices;
        private List<LCC3Material> _listOfMaterials;
        private LCC3ShaderProgram _materialShaderProgram;
        private LCC3NodeDrawingVisitor _drawingVisitor;


        #region Constructors

        public ShowcaseGameScene(Game game, CC3GraphicsContext graphicsContext) : base(graphicsContext)
        {
            /*
            this.InitializeCamera();
            this.InitializeEffect();
            this.InitializeCubeDrawingData();
            this.InitializeCubes();
            this.InitializeCameraAction();
            */


            _progPipeline = LCC3ProgPipeline.SharedPipeline(game);

            /*
            this.InitializeCameraForTextureTest();
            this.InitializeModel();
            this.InitializeTankEffect();
            this.RunCameraActionForTextureTest();
            */

            this.InitializeCameraForMaterialTest();
            this.InitializeMaterialCubes();
            this.InitializeMaterialShader();
            this.InitializeMaterials();
            this.InitializeMaterialDrawingData();
        }

        #endregion Constructors


        #region Material test

        private void InitializeCameraForMaterialTest()
        {
            CC3Vector cameraPos = new CC3Vector(0.0f, 10.0f, 20.0f);
            CC3Vector cameraTarget = new CC3Vector(0.0f, 0.0f, 0.0f);
            float cameraFieldOfViewInDegrees = 80.0f;
            float cameraAspectRatio = _graphicsContext.ScreenAspectRatio;
            float cameraNearClippingDistance = 1.0f;
            float cameraFarClippingDistance = 10000.0f;

            CC3CameraPerspectiveBuilder cameraBuilder = new CC3CameraPerspectiveBuilder();
            cameraBuilder.PositionAtPoint(cameraPos).LookingAtPoint(cameraTarget);
            cameraBuilder.WithFieldOfView(cameraFieldOfViewInDegrees).WithAspectRatio(cameraAspectRatio);
            cameraBuilder.WithNearAndFarClippingDistances(cameraNearClippingDistance, cameraFarClippingDistance);

            _camera = cameraBuilder.Build();

            this.ActiveCamera = _camera;
        }

        private void InitializeMaterialShader()
        {
            _progPipeline.EnableVertexAttributeAtIndex(true, LCC3VertexAttrIndex.VertexAttribPosition);
            _progPipeline.EnableVertexAttributeAtIndex(true, LCC3VertexAttrIndex.VertexAttribNormal);
            //_progPipeline.EnableVertexAttributeAtIndex(true, LCC3VertexAttrIndex.VertexAttribTexCoords);
            _progPipeline.EnableVertexAttributeAtIndex(true, LCC3VertexAttrIndex.VertexAttribColor);

            LCC3ShaderProgramMatchers programMatchers = new LCC3ShaderProgramMatchers();
            _materialShaderProgram = programMatchers.ConfigurableProgram();
        }

        private void InitializeMaterials()
        {
            _listOfMaterials = new List<LCC3Material>();

            LCC3Material material = new LCC3Material(0, "Glowy");
            material.ShaderProgram = _materialShaderProgram;
            material.AmbientColor = new CCColor4F(0.0f, 0.1f, 0.1f, 1.0f);
            material.DiffuseColor = new CCColor4F(0.5f, 0.5f, 1.0f, 1.0f);
            material.SpecularColor = new CCColor4F(1.0f, 1.0f, 0.0f, 1.0f);
            material.EmissionColor = new CCColor4F(0.5f, 0.5f, 0.0f, 1.0f);
            material.Shininess = 100.0f;
            material.ShouldUseLighting = true;

            _listOfMaterials.Add(material);

            material = new LCC3Material(1, "Grass");
            material.ShaderProgram = _materialShaderProgram;
            material.AmbientColor = new CCColor4F(0.0f, 0.1f, 0.0f, 1.0f);
            material.DiffuseColor = new CCColor4F(0.0f, 0.2f, 0.0f, 0.2f);
            material.SpecularColor = new CCColor4F(0.0f, 1.0f, 0.0f, 1.0f);
            material.EmissionColor = new CCColor4F(0.0f, 0.1f, 0.0f, 1.0f);
            material.Shininess = 1.0f;
            material.ShouldUseLighting = true;

            _listOfMaterials.Add(material);

            material = new LCC3Material(2, "Metal");
            material.ShaderProgram = _materialShaderProgram;
            material.AmbientColor = new CCColor4F(0.0f, 0.3f, 0.9f, 1.0f);
            material.DiffuseColor = new CCColor4F(0.0f, 0.0f, 1.0f, 1.0f);
            material.SpecularColor = new CCColor4F(0.1f, 0.8f, 0.5f, 1.0f);
            material.EmissionColor = new CCColor4F(0.0f, 0.2f, 1.0f, 1.0f);
            material.Shininess = 100.0f;
            material.ShouldUseLighting = true;

            _listOfMaterials.Add(material);

            material = new LCC3Material(3, "Watery");
            material.ShaderProgram = _materialShaderProgram;
            material.AmbientColor = new CCColor4F(0.1f, 0.9f, 0.0f, 1.0f);
            material.DiffuseColor = new CCColor4F(0.9f, 0.5f, 0.9f, 0.5f);
            material.SpecularColor = new CCColor4F(0.0f, 0.0f, 0.9f, 1.0f);
            material.EmissionColor = new CCColor4F(0.1f, 0.15f, 0.15f, 1.0f);
            material.Shininess = 1.0f;
            material.ShouldUseLighting = true;

            _listOfMaterials.Add(material);
        }

        private void InitializeMaterialCubes()
        {
            _listOfMaterialCubeMatrices = new List<LCC3Matrix4x4>();

            Vector3 cubePosition = new Vector3(-15.0f, 0.0f, 0.0f);

            for(int i=0; i < _numOfMaterialCubes; i++)
            {
                _listOfMaterialCubeMatrices.Add(
                    new LCC3Matrix4x4(Matrix.CreateRotationY(MathHelper.ToRadians(45.0f)) * Matrix.CreateTranslation(cubePosition)));
                cubePosition.X += 10.0f;
                cubePosition.Z += 0.0f;
            }
        }

        private void InitializeMaterialDrawingData()
        {
            uint numOfVertices = 18;
            LCC3Mesh mesh = new LCC3Mesh();
            LCC3VertexLocations locations = new LCC3VertexLocations(1, "locations");
            locations.AllocateVertexCapacity(numOfVertices);
            // Top face
            locations.SetLocation(new LCC3Vector(0.0f, 0.0f, 0.0f), 0);
            locations.SetLocation(new LCC3Vector(0.0f, 0.0f, 5.0f), 1);
            locations.SetLocation(new LCC3Vector(5.0f, 0.0f, 0.0f), 2);

            locations.SetLocation(new LCC3Vector(5.0f, 0.0f, 0.0f), 3);
            locations.SetLocation(new LCC3Vector(5.0f, 0.0f, 5.0f), 4);
            locations.SetLocation(new LCC3Vector(0.0f, 0.0f, 5.0f), 5);

            // Side front face
            locations.SetLocation(new LCC3Vector(0.0f, 0.0f, 5.0f), 6);
            locations.SetLocation(new LCC3Vector(0.0f, -5.0f, 5.0f), 7);
            locations.SetLocation(new LCC3Vector(5.0f, 0.0f, 5.0f), 8);

            locations.SetLocation(new LCC3Vector(5.0f, 0.0f, 5.0f), 9);
            locations.SetLocation(new LCC3Vector(5.0f, -5.0f, 5.0f), 10);
            locations.SetLocation(new LCC3Vector(0.0f, -5.0f, 5.0f), 11);

            // Side left face
            locations.SetLocation(new LCC3Vector(0.0f, 0.0f, 0.0f), 12);
            locations.SetLocation(new LCC3Vector(0.0f, -5.0f, 0.0f), 13);
            locations.SetLocation(new LCC3Vector(0.0f, 0.0f, 5.0f), 14);

            locations.SetLocation(new LCC3Vector(0.0f, 0.0f, 5.0f), 15);
            locations.SetLocation(new LCC3Vector(0.0f, -5.0f, 5.0f), 16);
            locations.SetLocation(new LCC3Vector(0.0f, -5.0f, 0.0f), 17);

            LCC3VertexNormals normals = new LCC3VertexNormals(1, "normals");
            normals.AllocateVertexCapacity(numOfVertices);
            // Top face normals         
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, 1.0f, -1.0f).NormalizedVector(), 0);
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, 1.0f, 1.0f).NormalizedVector(), 1);
            normals.SetNormalAtIndex(new LCC3Vector(1.0f, 1.0f, -1.0f).NormalizedVector(), 2);

            normals.SetNormalAtIndex(new LCC3Vector(1.0f, 1.0f, -1.0f).NormalizedVector(), 3);
            normals.SetNormalAtIndex(new LCC3Vector(1.0f, 1.0f, 1.0f).NormalizedVector(), 4);
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, 1.0f, 1.0f).NormalizedVector(), 5);

            // Side front face normals
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, 1.0f, 1.0f).NormalizedVector(), 6);
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, -1.0f, 1.0f).NormalizedVector(), 7);
            normals.SetNormalAtIndex(new LCC3Vector(1.0f, 1.0f, 1.0f).NormalizedVector(), 8);

            normals.SetNormalAtIndex(new LCC3Vector(1.0f, 1.0f, 1.0f).NormalizedVector(), 9);
            normals.SetNormalAtIndex(new LCC3Vector(1.0f, -1.0f, 1.0f).NormalizedVector(), 10);
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, -1.0f, 1.0f).NormalizedVector(), 11);

            // Side left face normals
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, 1.0f, -1.0f).NormalizedVector(), 12);
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, -1.0f, -1.0f).NormalizedVector(), 13);
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, 1.0f, 1.0f).NormalizedVector(), 14);

            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, 1.0f, 1.0f).NormalizedVector(), 15);
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, -1.0f, 1.0f).NormalizedVector(), 16);
            normals.SetNormalAtIndex(new LCC3Vector(-1.0f, -1.0f, -1.0f).NormalizedVector(), 17);

            /*
            for (uint i=0; i < 6; i++)
            {
                normals.SetNormalAtIndex(new LCC3Vector(0.0f, 1.0f, 0.0f).NormalizedVector(), i);
                normals.SetNormalAtIndex(new LCC3Vector(0.0f, 0.0f, 1.0f).NormalizedVector(), i + 6);
                normals.SetNormalAtIndex(new LCC3Vector(-1.0f, 0.0f, 0.0f).NormalizedVector(), i + 12);
            }
            */

            LCC3VertexTextureCoordinates texCoords = new LCC3VertexTextureCoordinates(0, "texCoords");
            texCoords.AllocateVertexCapacity(numOfVertices);
            for (uint i=0; i < numOfVertices / 6; i++)
            {
                uint offset = i * 6;
                texCoords.SetTexCoord2FAtIndex(new CCTex2F(0.0f, 0.0f), 0 + offset);
                texCoords.SetTexCoord2FAtIndex(new CCTex2F(0.0f, 1.0f), 1 + offset);
                texCoords.SetTexCoord2FAtIndex(new CCTex2F(1.0f, 0.0f), 2 + offset);

                texCoords.SetTexCoord2FAtIndex(new CCTex2F(1.0f, 0.0f), 3 + offset);
                texCoords.SetTexCoord2FAtIndex(new CCTex2F(1.0f, 1.0f), 4 + offset);
                texCoords.SetTexCoord2FAtIndex(new CCTex2F(0.0f, 1.0f), 5 + offset);
            }

            LCC3VertexColors colors = new LCC3VertexColors(0, "colors");
            colors.AllocateVertexCapacity(numOfVertices);
            for (uint i=0; i < numOfVertices; i++)
            {
                colors.SetColor4FAtIndex(new CCColor4F(0.2f, 1.0f, 0.0f, 1.0f), i);
            }

            mesh.VertexLocations = locations;
            mesh.VertexNormals = normals;
            //mesh.VertexTextureCoords = texCoords;
            mesh.VertexColors = colors;

            LCC3Scene scene = new LCC3Scene();
            scene.AmbientLight = new CCColor4F(0.5f, 0.5f, 0.0f, 0.5f);

            LCC3Material defaultMaterial = new LCC3Material(0, "Default");
            defaultMaterial.ShaderProgram = _materialShaderProgram;
            defaultMaterial.AmbientColor = new CCColor4F(0.2f, 1.0f, 0.0f, 1.0f);
            defaultMaterial.DiffuseColor = new CCColor4F(0.2f, 1.0f, 0.0f, 0.0f);
            defaultMaterial.SpecularColor = new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f);
            defaultMaterial.EmissionColor = new CCColor4F(0.2f, 1.0f, 0.0f, 1.0f);
            defaultMaterial.Shininess = 0.0f;
            defaultMaterial.ShouldUseLighting = true;

            LCC3MeshNode meshNode = new LCC3MeshNode();
            meshNode.Material = defaultMaterial;
            meshNode.Mesh = mesh;
            meshNode.Parent = scene;

            LCC3Light[] lights = new LCC3Light[(int)LCC3Light.DefaultMaxNumOfLights];
            for (int i = 0; i < LCC3Light.DefaultMaxNumOfLights; i++)
            {
                lights[i] = new LCC3Light();
                lights[i].Visible = true;
                lights[i].Location = (new LCC3Vector(0.0f, -1.0f, -0.2f)).NormalizedVector();
                lights[i].AmbientColor = new CCColor4F(0.0f, 0.5f, 0.0f, 1.0f);
                lights[i].DiffuseColor = new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f);
                lights[i].SpecularColor = new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f);
                }

            scene.Lights = lights;

            _drawingVisitor = new LCC3NodeDrawingVisitor();
            _drawingVisitor.CurrentColor = new CCColor4F(1.0f, 0.0f, 0.0f, 1.0f);
            _drawingVisitor.CurrentNode = meshNode;
            _drawingVisitor.StartingNode = meshNode;

            _drawingVisitor.ModelMatrix = LCC3Matrix4x4.CC3MatrixIdentity;
            _drawingVisitor.ViewMatrix = new LCC3Matrix4x4(_graphicsContext.ViewMatrix.XnaMatrix);
            _drawingVisitor.ProjMatrix = new LCC3Matrix4x4(_graphicsContext.ProjectionMatrix.XnaMatrix);

            _drawingVisitor.ProgramPipeline = _progPipeline;

            _progPipeline.BindProgramWithVisitor(_materialShaderProgram, _drawingVisitor);

            /*
            _tankTexture = new LCC3GraphicsTexture2D("turret_alt_diff_tex_0");

            LCC3ShaderUniform textureUniform = _materialShaderProgram.UniformNamed("Texture");
            textureUniform.SetValue(_tankTexture);
            textureUniform.UpdateShaderValue();
            */

            _progPipeline.GenerateVertexBuffer();

        }

        
        public void DrawMaterialTest()
        {
            _progPipeline.SetClearColor(new CCColor4F(0.2f, 0.5f, 0.8f, 1.0f));
            _progPipeline.SetClearDepth(100.0f);
            _progPipeline.ClearBuffers(LCC3BufferMask.ColorBuffer | LCC3BufferMask.DepthBuffer);

            _progPipeline.EnableBlend(false);
            _progPipeline.EnableDepthTest(true);
            _progPipeline.SetDepthMask(true);
            _progPipeline.SetDepthFunc(LCC3DepthStencilFuncMode.LessOrEqual);
            _progPipeline.EnableCullFace(true);

            for (int i=0; i < _numOfMaterialCubes; i++)
            {
                _drawingVisitor.ModelMatrix = _listOfMaterialCubeMatrices[i];
                _drawingVisitor.ViewMatrix = new LCC3Matrix4x4(_graphicsContext.ViewMatrix.XnaMatrix);
                _drawingVisitor.ProjMatrix = new LCC3Matrix4x4(_graphicsContext.ProjectionMatrix.XnaMatrix);
                _drawingVisitor.CurrentMeshNode.Material = _listOfMaterials[i];
                _progPipeline.BindProgramWithVisitor(_materialShaderProgram, _drawingVisitor);
                _progPipeline.DrawVertices(LCC3DrawMode.TriangleList, 0, 6);
            }
        }

        #endregion Material test


        #region Legacy texture test

        private void InitializeCameraForTextureTest()
        {
            CC3Vector cameraPos = new CC3Vector(750.0f, 1000.0f, 10.0f);
            CC3Vector cameraTarget = new CC3Vector(0.0f, 300.0f, 0.0f);
            float cameraFieldOfViewInDegrees = 45.0f;
            float cameraAspectRatio = _graphicsContext.ScreenAspectRatio;
            float cameraNearClippingDistance = 200.0f;
            float cameraFarClippingDistance = 10000.0f;

            CC3CameraPerspectiveBuilder cameraBuilder = new CC3CameraPerspectiveBuilder();
            cameraBuilder.PositionAtPoint(cameraPos).LookingAtPoint(cameraTarget);
            cameraBuilder.WithFieldOfView(cameraFieldOfViewInDegrees).WithAspectRatio(cameraAspectRatio);
            cameraBuilder.WithNearAndFarClippingDistances(cameraNearClippingDistance, cameraFarClippingDistance);

            _camera = cameraBuilder.Build();
          
            this.ActiveCamera = _camera;
        }

        private void InitializeModel()
        {
            _tankModel = _progPipeline.XnaGame.Content.Load<Model>("tank");
            _tankTransforms = new Matrix[_tankModel.Bones.Count];
            _tankModel.CopyAbsoluteBoneTransformsTo(_tankTransforms);
        }

        private void InitializeTankEffect()
        {
            _tankShader = new LCC3ShaderProgram(0, "MyShader", this, "Content/BasicEffect.ogl.mgfxo");
            _tankTexture = new LCC3GraphicsTexture2D("turret_alt_diff_tex_0");
            _tankShader.XnaShaderEffect.CurrentTechnique = _tankShader.XnaShaderEffect.Techniques[21]; //5

            LCC3ShaderUniform textureUniform = _tankShader.UniformNamed("Texture");
            textureUniform.SetValue(_tankTexture);
            textureUniform.UpdateShaderValue();

            LCC3ShaderUniform diffuseColorUniform = _tankShader.UniformNamed("DiffuseColor");
            LCC3Vector4 diffuseColor = new LCC3Vector4(1.0f, 1.0f, 1.0f, 0.4f);
            diffuseColorUniform.SetValue(diffuseColor);
            diffuseColorUniform.UpdateShaderValue();

            LCC3ShaderUniform emissiveColorUniform = _tankShader.UniformNamed("EmissiveColor");
            LCC3Vector emissiveColor = new LCC3Vector(0.5f, 0.5f, 0.9f);
            emissiveColorUniform.SetValue(emissiveColor);
            emissiveColorUniform.UpdateShaderValue();

            LCC3ShaderUniform specularColorUniform = _tankShader.UniformNamed("SpecularColor");
            LCC3Vector specularColor = new LCC3Vector(1.0f, 0.0f, 0.0f);
            specularColorUniform.SetValue(specularColor);
            specularColorUniform.UpdateShaderValue();

            LCC3ShaderUniform specularPowerUniform = _tankShader.UniformNamed("SpecularPower");
            float specularPower = 100.0f;
            specularPowerUniform.SetValue(specularPower);
            specularPowerUniform.UpdateShaderValue();

            LCC3ShaderUniform light0DirectionUniform = _tankShader.UniformNamed("DirLight0Direction");
            LCC3Vector light0Direction = (new LCC3Vector(-0.5f, -1.2f, -1.0f)).NormalizedVector();
            light0DirectionUniform.SetValue(light0Direction);
            light0DirectionUniform.UpdateShaderValue();

            LCC3ShaderUniform light0DiffuseColorUniform = _tankShader.UniformNamed("DirLight0DiffuseColor");
            LCC3Vector light0DiffuseColor = new LCC3Vector(1.0f, 2.0f, 2.0f);
            light0DiffuseColorUniform.SetValue(light0DiffuseColor);
            light0DiffuseColorUniform.UpdateShaderValue();

            LCC3ShaderUniform light0SpecularColorUniform = _tankShader.UniformNamed("DirLight0SpecularColor");
            LCC3Vector light0SpecularColor = new LCC3Vector(1.0f, 0.0f, 0.0f);
            light0SpecularColorUniform.SetValue(light0SpecularColor);
            light0DiffuseColorUniform.UpdateShaderValue();
        }

        // ILCC3SemanticDelegate methods

        public bool ConfigureVariable(LCC3ShaderVariable variable)
        {
            if (variable.Name == "WorldViewProj")
                variable.Type = LCC3ElementType.Float4x4;
            else if (variable.Name == "World")
                variable.Type = LCC3ElementType.Float4x4;
            else if (variable.Name == "WorldInverseTranspose")
                variable.Type = LCC3ElementType.Float4x4;
            else if (variable.Name == "Texture")
                variable.Type = LCC3ElementType.Texture2D;
            else if (variable.Name == "DiffuseColor")
                variable.Type = LCC3ElementType.Vector4;
            else if (variable.Name == "SpecularColor")
                variable.Type = LCC3ElementType.Vector3;
            else if (variable.Name == "EmissiveColor")
                variable.Type = LCC3ElementType.Vector3;
            else if (variable.Name == "SpecularPower")
                variable.Type = LCC3ElementType.Float;
            else if (variable.Name == "DirLight0Direction")
                variable.Type = LCC3ElementType.Vector3;
            else if (variable.Name == "DirLight0DiffuseColor")
                variable.Type = LCC3ElementType.Vector3;
            else if (variable.Name == "DirLight0SpecularColor")
                variable.Type = LCC3ElementType.Vector3;

            return true;
        }
       
        public bool PopulateUniformWithVisitor(LCC3ShaderUniform uniform, LCC3NodeDrawingVisitor visitor)
        {
            return false;
        }

        // end of ILCC3SemanticDelegate methods

        private void RunCameraActionForTextureTest()
        {
            CC3CameraPerspectiveActionBuilder cameraActionBuilder 
                = new CC3CameraPerspectiveActionBuilder();
            cameraActionBuilder.RotateCameraAroundAxisRelativeToTargetByDegrees(new CC3Vector(0.0f, 1.0f, 0.0f), 360.0f);

            CC3CameraPerspectiveActionRunner runner 
                = new CC3CameraPerspectiveActionRunner(cameraActionBuilder.Build(), _camera, 4.0f);

            runner.RunAction();
        }

        public void DrawTextureTest()
        {
            _progPipeline.SetClearColor(new CCColor4F(0.2f, 0.5f, 0.8f, 1.0f));
            _progPipeline.SetClearDepth(100.0f);
            _progPipeline.ClearBuffers(LCC3BufferMask.ColorBuffer | LCC3BufferMask.DepthBuffer);
           
            _progPipeline.EnableBlend(false);
            _progPipeline.EnableDepthTest(true);
            _progPipeline.SetDepthMask(true);
            _progPipeline.SetDepthFunc(LCC3DepthStencilFuncMode.LessOrEqual);
            _progPipeline.EnableCullFace(true);


            _progPipeline.CurrentlyActiveShader = _tankShader;


            foreach (ModelMesh mesh in _tankModel.Meshes)
            {                
                foreach (ModelMeshPart part in mesh.MeshParts)
                {

                    LCC3ShaderUniform worldViewProjUniform = _tankShader.UniformNamed("WorldViewProj");
                    LCC3Matrix4x4 worldViewProjMatrix 
                        = new LCC3Matrix4x4(_tankTransforms[mesh.ParentBone.Index] * _graphicsContext.ViewMatrix.XnaMatrix * _graphicsContext.ProjectionMatrix.XnaMatrix);
                    worldViewProjUniform.SetValue(worldViewProjMatrix);
                    worldViewProjUniform.UpdateShaderValue();

                    LCC3ShaderUniform eyePositionUniform = _tankShader.UniformNamed("EyePosition");
                    LCC3Vector eyePosition = new LCC3Vector(Matrix.Invert(_graphicsContext.ViewMatrix.XnaMatrix).Translation);
                    eyePositionUniform.SetValue(eyePosition);
                    eyePositionUniform.UpdateShaderValue();

                    LCC3ShaderUniform worldUniform = _tankShader.UniformNamed("World");
                    LCC3Matrix4x4 worldMatrix = new LCC3Matrix4x4(_tankTransforms[mesh.ParentBone.Index]);
                    worldUniform.SetValue(worldMatrix);
                    worldUniform.UpdateShaderValue();


                    LCC3ShaderUniform worldInvTransUniform = _tankShader.UniformNamed("WorldInverseTranspose");
                    LCC3Matrix4x4 worldInvTransMatrix = new LCC3Matrix4x4(Matrix.Invert(Matrix.Transpose(_tankTransforms[mesh.ParentBone.Index])));
                    worldInvTransUniform.SetValue(worldInvTransMatrix);
                    worldInvTransUniform.UpdateShaderValue();

                    _progPipeline.XnaVertexBuffer = part.VertexBuffer;
                    _progPipeline.XnaIndexBuffer = part.IndexBuffer;

                    _progPipeline.DrawIndices(LCC3DrawMode.TriangleList, part.VertexOffset, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                }
            }
        }

        #endregion Legacy texture test


        #region Initializing scene

        private void InitializeCamera()
        {
            CC3Vector cameraPos = new CC3Vector(0.0f, 10.0f, 5.0f);
            CC3Vector cameraTarget = new CC3Vector(0.0f, 0.0f, -10.0f);
            float cameraFieldOfViewInDegrees = 60.0f;
            float cameraAspectRatio = _graphicsContext.ScreenAspectRatio;
            float cameraNearClippingDistance = 1.0f;
            float cameraFarClippingDistance = 10000.0f;


            CC3CameraPerspectiveBuilder cameraBuilder = new CC3CameraPerspectiveBuilder();
            cameraBuilder.PositionAtPoint(cameraPos).LookingAtPoint(cameraTarget);
            cameraBuilder.WithFieldOfView(cameraFieldOfViewInDegrees).WithAspectRatio(cameraAspectRatio);
            cameraBuilder.WithNearAndFarClippingDistances(cameraNearClippingDistance, cameraFarClippingDistance);

            _camera = cameraBuilder.Build();

            // Replace above block with below to instead use an orthographic camera
            /*
            CC3CameraOrthographicBuilder cameraBuilder = new CC3CameraOrthographicBuilder();
            cameraBuilder.PositionAtPoint(cameraPos).LookingAtPoint(cameraTarget);
            cameraBuilder.WithViewWidth(10.0f * cameraAspectRatio).WithViewHeight(10.0f);
            cameraBuilder.WithNearAndFarClippingDistances(cameraNearClippingDistance, cameraFarClippingDistance);

            _camera = cameraBuilder.Build() as CC3CameraOrthographic;
            */

            this.ActiveCamera = _camera;
        }

        private void InitializeCameraAction()
        {
            CC3CameraPerspectiveActionBuilder cameraActionBuilder = new CC3CameraPerspectiveActionBuilder();

            List<CC3CameraPerspectiveAction> listOfCameraActions = new List<CC3CameraPerspectiveAction>();

            cameraActionBuilder.PanCameraLeftByAmount(10.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.PanCameraRightByAmount(10.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.PanCameraUpByAmount(10.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.PanCameraDownByAmount(10.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.SetCameraFieldOfViewChangeInDegrees(-600.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.SetCameraFieldOfViewChangeInDegrees(600.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.RotateCameraAroundAxisRelativeToTargetByDegrees(new CC3Vector(0.0f, 1.0f, 0.0f), 360.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.RotateCameraAroundAxisRelativeToTargetByDegrees(new CC3Vector(1.0f, 0.0f, 0.0f), 360.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.RotateCameraAroundAxisRelativeToTargetByDegrees(new CC3Vector(0.0f, 0.0f, 1.0f), 360.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.RotateCameraAroundAxisRelativeToTargetByDegrees(new CC3Vector(1.0f, 0.0f, 1.0f), 360.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.RotateCameraAroundAxisRelativeToTargetByDegrees(new CC3Vector(0.0f, 1.0f, 1.0f), 360.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            cameraActionBuilder.RotateCameraAroundAxisRelativeToTargetByDegrees(new CC3Vector(1.0f, 1.0f, 0.0f), 360.0f);
            listOfCameraActions.Add(cameraActionBuilder.Build());
            cameraActionBuilder.Reset();

            CC3SequenceActionRunner sequenceRunner = new CC3SequenceActionRunner(12.0f);

            foreach (CC3CameraPerspectiveAction action in listOfCameraActions)
            {
                sequenceRunner.AddActionWithTarget(action, _camera, 1.0f);
            }

            sequenceRunner.RunAction();

        }

        private void InitializeEffect()
        {
            GraphicsDevice graphicsDevice = _graphicsContext.XnaGraphicsDeviceManager.GraphicsDevice;
            _basicEffect = new BasicEffect( graphicsDevice );
            _basicEffect.Alpha = 1.0f;
            _basicEffect.TextureEnabled = false;
            _basicEffect.VertexColorEnabled = true;


            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.BlendState = BlendState.Opaque;
        }

        private void InitializeCubeDrawingData()
        {
            Vector3 topLeftFront = new Vector3( -1.0f, 1.0f, 1.0f );
            Vector3 bottomLeftFront = new Vector3( -1.0f, -1.0f, 1.0f );
            Vector3 topRightFront = new Vector3( 1.0f, 1.0f, 1.0f );
            Vector3 bottomRightFront = new Vector3( 1.0f, -1.0f, 1.0f );
            Vector3 topLeftBack = new Vector3( -1.0f, 1.0f, -1.0f );
            Vector3 topRightBack = new Vector3( 1.0f, 1.0f, -1.0f );
            Vector3 bottomLeftBack = new Vector3( -1.0f, -1.0f, -1.0f );
            Vector3 bottomRightBack = new Vector3( 1.0f, -1.0f, -1.0f );

            VertexPositionColor[] cubeVertices = new VertexPositionColor[8];

            cubeVertices[0] =
                new VertexPositionColor(
                    bottomLeftFront, Color.Aqua );
            cubeVertices[1] =
                new VertexPositionColor(
                    topLeftFront, Color.AliceBlue );
            cubeVertices[2] =
                new VertexPositionColor (
                    bottomRightFront, Color.Aquamarine );
            cubeVertices[3] =
                new VertexPositionColor(
                    topRightFront, Color.CadetBlue );
            cubeVertices[4] =
                new VertexPositionColor(
                    bottomRightBack, Color.Blue );
            cubeVertices[5] =
                new VertexPositionColor(
                    topRightBack, Color.LightYellow );
            cubeVertices[6] =
                new VertexPositionColor(
                    bottomLeftBack, Color.LightGoldenrodYellow );
            cubeVertices[7] =
                new VertexPositionColor(
                    topLeftBack, Color.YellowGreen );

            short[] cubeVertexIndices = new short[]{0,1,2,3,4,5,6,7,4,2,6,0,7,1,5,3};


            GraphicsDevice graphicsDevice = _graphicsContext.XnaGraphicsDeviceManager.GraphicsDevice;

            _vertexBuffer = new VertexBuffer(graphicsDevice, 
                                             typeof(VertexPositionColor), 
                                             cubeVertices.Length, 
                                             BufferUsage.None);

            _indexBuffer = new IndexBuffer(graphicsDevice, 
                                           IndexElementSize.SixteenBits, 
                                           sizeof(short) * cubeVertexIndices.Length, 
                                           BufferUsage.WriteOnly);

            _indexBuffer.SetData(cubeVertexIndices);
            _vertexBuffer.SetData<VertexPositionColor>( cubeVertices );
        }

        private void InitializeCubes()
        {
            _listOfCubeWorldMatrices = new List<Matrix>();

            Vector3 cubePosition = new Vector3(-5.0f, 0.0f, -10.0f);

            for(int i=0; i < 4; i++)
            {
                _listOfCubeWorldMatrices.Add(
                Matrix.CreateRotationY(MathHelper.ToRadians(45.0f)) * Matrix.CreateTranslation(cubePosition));
                cubePosition.X += 4.0f;
                cubePosition.Z += 0.0f;
            }
        }

        #endregion Initializing scene

        private void DrawCoreCameraTest()
        {
            _graphicsContext.ClearColor = new CCColor4F(0.2f, 0.5f, 0.8f, 1.0f);
            _graphicsContext.ClearColorBuffer();

            GraphicsDevice graphicsDevice = _graphicsContext.XnaGraphicsDeviceManager.GraphicsDevice;

            graphicsDevice.RasterizerState = RasterizerState.CullNone;

            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.BlendState = BlendState.Opaque;

            graphicsDevice.Indices = _indexBuffer;
            graphicsDevice.SetVertexBuffer(_vertexBuffer);

            foreach (Matrix cubeWorldMatrix in _listOfCubeWorldMatrices)
            {
                _basicEffect.World = cubeWorldMatrix;
                _basicEffect.View = _graphicsContext.ViewMatrix.XnaMatrix;
                _basicEffect.Projection = _graphicsContext.ProjectionMatrix.XnaMatrix;

                foreach (EffectPass effectPass in _basicEffect.CurrentTechnique.Passes)
                {
                    effectPass.Apply();
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, 8, 0, 6);
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, 8, 8, 6); 
                }
            }
        }

        public override void Draw()
        {
            //this.DrawCoreCameraTest();

            //this.DrawTextureTest();

            this.DrawMaterialTest();
        }

    }
}

