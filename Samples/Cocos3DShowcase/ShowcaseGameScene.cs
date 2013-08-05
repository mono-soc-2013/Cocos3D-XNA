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


        #region Constructors

        public ShowcaseGameScene(Game game, CC3GraphicsContext graphicsContext) : base(graphicsContext)
        {

            this.InitializeCamera();
            this.InitializeEffect();
            this.InitializeCubeDrawingData();
            this.InitializeCubes();
            this.InitializeCameraAction();


            /*
            _progPipeline = LCC3ProgPipeline.SharedPipeline(game);

            this.InitializeCameraForTextureTest();
            this.InitializeModel();
            this.InitializeTankEffect();
            this.RunCameraActionForTextureTest();
            */
        }

        #endregion Constructors


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
            _tankShader.ConfigureUniforms();
            _tankTexture = new LCC3GraphicsTexture2D("turret_alt_diff_tex_0");
            _tankShader.XnaShaderEffect.CurrentTechnique = _tankShader.XnaShaderEffect.Techniques[5];

            LCC3ShaderUniform textureUniform = _tankShader.UniformNamed("Texture");
            textureUniform.SetValue(_tankTexture);
            textureUniform.UpdateShaderValue();

            LCC3ShaderUniform diffuseColorUniform = _tankShader.UniformNamed("DiffuseColor");
            LCC3Vector4 diffuseColor = LCC3Vector4.CC3Vector4One;
            diffuseColorUniform.SetValue(diffuseColor);
            diffuseColorUniform.UpdateShaderValue();

            LCC3ShaderUniform specularColorUniform = _tankShader.UniformNamed("SpecularColor");
            LCC3Vector specularColor = LCC3Vector.CC3VectorUnitCube;
            specularColorUniform.SetValue(specularColor);
            specularColorUniform.UpdateShaderValue();

            LCC3ShaderUniform specularPowerUniform = _tankShader.UniformNamed("SpecularPower");
            float specularPower = 16.0f;
            specularPowerUniform.SetValue(specularPower);
            specularPowerUniform.UpdateShaderValue();
        }

        // ILCC3SemanticDelegate methods

        public bool ConfigureVariable(LCC3ShaderVariable variable)
        {
            if (variable.Name == "WorldViewProj")
                variable.Type = LCC3ShaderVariableType.Matrix;
            else if (variable.Name == "Texture")
                variable.Type = LCC3ShaderVariableType.Texture2D;
            else if (variable.Name == "DiffuseColor")
                variable.Type = LCC3ShaderVariableType.Vector4;
            else if (variable.Name == "SpecularColor")
                variable.Type = LCC3ShaderVariableType.Vector3;
            else if (variable.Name == "SpecularPower")
                variable.Type = LCC3ShaderVariableType.Float;

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

                    _progPipeline.XnaVertexBuffer = part.VertexBuffer;
                    _progPipeline.XnaIndexBuffer = part.IndexBuffer;
                    _progPipeline.BindVertexBuffer();
                    _progPipeline.BindIndexBuffer();
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
            this.DrawCoreCameraTest();

            //this.DrawTextureTest();
        }

    }
}

