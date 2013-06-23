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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3D
{
    public class CC3GraphicsContext
    {
        // Static fields

        private static readonly Color _defaultXnaClearColor = Color.Black;
        private static readonly float _defaultClearDepth = 1.0f;
        private static readonly int _defaultClearStencil = 0;

        // Instance fields

        private GraphicsDeviceManager _xnaGraphicsDeviceManager;

        private Color _xnaClearColor;
        private float _clearDepth;
        private int _clearStencil;

        private CC3Matrix _viewMatrix;
        private CC3Matrix _projectionMatrix;

        #region Properties


        // Instance properties

        public float ScreenAspectRatio
        {
            get { return _xnaGraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio; }
        }

        public CCColor4F ClearColor
        {
            set { _xnaClearColor = value.XnaColor(); }
        }

        public float ClearDepth
        {
            set { _clearDepth = value; }
        }

        public int ClearStencil
        {
            set { _clearStencil = value; }
        }

        public CC3Matrix ViewMatrix
        {
            get { return _viewMatrix; }
            internal set { _viewMatrix = value; }
        }

        public CC3Matrix ProjectionMatrix
        {
            get { return _projectionMatrix; }
            internal set { _projectionMatrix = value; }
        }

        // For testing only. Should remove this altogether
        public GraphicsDeviceManager XnaGraphicsDeviceManager
        {
            get { return _xnaGraphicsDeviceManager; }
        }

        #endregion Properties


        #region Constructors

        public CC3GraphicsContext(GraphicsDeviceManager xnaGraphicsDeviceManager)
        {
            _xnaGraphicsDeviceManager = xnaGraphicsDeviceManager;

            _xnaClearColor = CC3GraphicsContext._defaultXnaClearColor;
            _clearDepth = CC3GraphicsContext._defaultClearDepth;
            _clearStencil = CC3GraphicsContext._defaultClearStencil;

            _viewMatrix = CC3Matrix.Identity;
            _projectionMatrix = CC3Matrix.Identity;
        }

        #endregion Constructors


        #region Clearing buffers

        public void ClearBuffers(CC3BufferMask bufferMask)
        {
            _xnaGraphicsDeviceManager.GraphicsDevice.Clear(bufferMask.XnaBufferMask(), _xnaClearColor, _clearDepth, _clearStencil);
        }

        public void ClearColorBuffer()
        {
            this.ClearBuffers(CC3BufferMask.ColorBuffer);
        }

        public void ClearDepthBuffer()
        {
            this.ClearBuffers(CC3BufferMask.DepthBuffer);
        }

        public void ClearStencilBuffer()
        {
            this.ClearBuffers(CC3BufferMask.StencilBuffer);
        }

        #endregion Clearing buffers
    }
}

