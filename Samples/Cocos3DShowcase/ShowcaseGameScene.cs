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
    public class ShowcaseGameScene : CC3Scene
    {
        // Instance fields

        private CC3CameraPerspective _camera;
        private CC3CameraPerspectiveAction _cameraAction;
        private CC3ActionRunner _cameraActionRunner;

        private BasicEffect _basicEffect;
        private List<Matrix> _listOfCubeWorldMatrices;

        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        #region Constructors

        public ShowcaseGameScene(CC3GraphicsContext graphicsContext) : base(graphicsContext)
        {
            this.InitializeCamera();
            this.InitializeEffect();
            this.InitializeCubeDrawingData();
            this.InitializeCubes();
            this.InitializeCameraAction();
        }

        #endregion Constructors


        #region Initializing scene

        private void InitializeCamera()
        {
            CC3Vector cameraPos = new CC3Vector(0.0f, 10.0f, 10.0f);
            CC3Vector cameraTarget = new CC3Vector(0.0f, 0.0f, -10.0f);
            float cameraFieldOfViewInDegrees = 60.0f;
            float cameraAspectRatio = _graphicsContext.ScreenAspectRatio;
            float cameraNearClippingDistance = 1.0f;
            float cameraFarClippingDistance = 1000.0f;


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
            CC3Vector cameraRotationAxis = (new CC3Vector(1.0f, 0.0f, 1.0f)).NormalizedVector();
            CC3Vector4 cameraActionRotaion = new CC3Vector4(cameraRotationAxis, 360.0f);


            _cameraAction = new CC3CameraPerspectiveAction(CC3Vector.CC3VectorZero, 
                                                           CC3Vector.CC3VectorZero,
                                                           cameraActionRotaion,                                                         
                                                           0.0f, 0.0f, 0.0f);
          
            _cameraActionRunner = new CC3CameraPerspectiveActionRunner(_cameraAction, _camera, 8.0f);

            _cameraActionRunner.RunAction();
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

            Vector3 cubePosition = new Vector3(-10.0f, 0.0f, -10.0f);

            for(int i=0; i < 4; i++)
            {
                _listOfCubeWorldMatrices.Add(
                Matrix.CreateRotationY(MathHelper.ToRadians(45.0f)) * Matrix.CreateTranslation(cubePosition));
                cubePosition.X += 4.0f;
                cubePosition.Z += 0.0f;
            }
        }

        #endregion Initializing scene

        public override void Draw()
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

    }
}

