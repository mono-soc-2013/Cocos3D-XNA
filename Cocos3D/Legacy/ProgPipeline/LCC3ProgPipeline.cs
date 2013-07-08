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
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Cocos2D;

namespace Cocos3D
{
    public class LCC3ProgPipeline
    {
        // Static fields

        private static LCC3ProgPipeline _sharedProgPipeline;

        // Instance fields

        private GraphicsDevice _xnaGraphicsDevice;
        private BlendState _xnaBlendState;
        private CullMode _xnaCullMode;
        private DepthStencilState _xnaDepthStencilState;
        private float _xnaDepthBias;
        private float _xnaSlopeScaleDepthBias;

        private List<LCC3VertexAttr> _vertexAttributes;

        private List<uint> _valueTextureBinding2d;
        private List<uint> _valueTextureBindingCubeMap;


        #region Convenience methods

        public static void SetCapability(bool value)
        {

        }

        #endregion Convenience methods


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
            _xnaGraphicsDevice = xnaGraphicsDevice;
            _xnaBlendState = BlendState.Opaque;
            _xnaCullMode = CullMode.None;
            _xnaDepthStencilState = DepthStencilState.None;
            _xnaDepthBias = 0.0f;
            _xnaSlopeScaleDepthBias = 0.0f;

            this.InitPlatformLimits();
            this.InitVertexAttributes();
            this.InitTextureUnits();
        }

        public void InitPlatformLimits()
        {
            // Get the GL vendor, renderer and version
            // Can't include in port
        }

        public void InitVertexAttributes()
        {
            _vertexAttributes = new List<LCC3VertexAttr>();
        }

        public void InitTextureUnits()
        {
            _valueTextureBinding2d = new List<uint>();
            _valueTextureBindingCubeMap = new List<uint>();
        }

        #endregion Allocation and initialization


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

        public void EnableDither(bool onOrOff)
        {
            // Dithering not supported by XNA
            throw new NotSupportedException("Dithering is not currently supported");
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

        }

        public void BindVertexAttributeWithVisitor(LCC3ShaderAttribute attribute, 
                                                   LCC3NodeDrawingVisitor visitor)
        {
        }

        public void EnableVertexAttributeAtIndex(bool onOrOff, int vaIndex)
        {

        }

        public LCC3VertexArray VertexArrayForAttributeWithVisitor(LCC3ShaderAttribute Attribute, 
                                                                  LCC3NodeDrawingVisitor visitor)
        {
            return null;
        }

        public void SetVertexAttributeEnablementAtIndex(int vaIndex)
        {

        }

        public void BindVertexContentToAttributeAtIndex(object pData, 
                                                        int elemSize, 
                                                        int elemType, 
                                                        int vertexStride, 
                                                        bool shouldNormalize, 
                                                        int vaIndex)
        {

        }

        public void BindVertexContentToAttributeAtIndex(int vaIndex)
        {

        }

        public void ClearUnboundVertexAttributes()
        {

        }

        public void EnableBoundVertexAttributes()
        {

        }

        public void Enable2DVertexAttributes()
        {

        }

        public int GenerateBuffer()
        {
            return 0;
        }

        public void DeleteBuffer(int buffId)
        {

        }

        public void BindBufferToTarget(int buffId, int target)
        {

        }

        public void UnbindBufferTarget(int target)
        {

        }

        public void LoadBufferTarget(int target, object buffData, long buffLength, int buffUsage)
        {

        }

        public void UpdateBufferTarget(int target, object buffData, long buffLength)
        {

        }

        public void DrawVertices(int drawMode, int startIndex, int length)
        {

        }

        public void DrawIndices(object indices, int length, int type, int drawMode)
        {

        }

        #endregion Vertex attributes


        #region State

        public void SetClearColor(CCColor4F color)
        {
        }

        public void SetClearDepth(float val)
        {

        }

        public void SetClearStencil(int val)
        {

        }

        public void SetColorMask(CCColor4F mask)
        {

        }

        public void SetCullFace(int val)
        {

        }

        public void SetDepthFunc(int val)
        {

        }

        public void SetDepthMask(bool writable)
        {

        }

        public void SetFrontFace(int val)
        {

        }

        public void SetLineWidth(float val)
        {
            
        }

        public void SetPolygonOffsetFactor(float factor, float units)
        {

        }

        public void SetScissor(LCC3Viewport viewport)
        {

        }

        public void SetStencilFunc(int func, int reference, uint mask)
        {

        }

        public void SetStencilMask(uint mask)
        {

        }

        public void SetOpOnStencilFail(int sFail, int zFail, int zPass)
        {

        }

        public void SetViewport(LCC3Viewport viewport)
        {

        }

        public void ClearBuffers(uint mask)
        {

        }

        public void ClearColorBuffer()
        {

        }

        public void ClearDepthBuffer()
        {

        }

        public void ClearStencilBuffer()
        {

        }

        #endregion State


        #region Materials

        public void SetBlendFuncSrcAndDst(uint src, uint dst)
        {

        }

        #endregion Materials


        #region Textures

        public uint GenerateTextureId()
        {
            return 0;
        }

        public void DeleteTextureId()
        {

        }

        public void LoadTextureImageIntoTarget(object imageData, 
                                               int target, 
                                               float width, 
                                               float height,
                                               int texelFormat,
                                               int texelType,
                                               int byteAlignment,
                                               uint texUnitIndex)
        {

        }

        public void ActivateTextureUnit(uint texUnitIndex)
        {

        }

        public void BindTextureToTarget(uint texId, uint target, uint texUnitIndex)
        {

        }

        public void SetTexParamEnum(uint pName, uint target, uint val, uint texUnitIndex)
        {

        }

        public void SetTextureMinifyFuncInTarget(uint func, uint target, uint texUnitIndex)
        {

        }

        public void SetTextureMagnifyFuncInTarget(uint func, uint target, uint texUnitIndex)
        {

        }

        public void SetTextureHorizWrapFuncInTarget(uint func, uint target, uint texUnitIndex)
        {

        }

        public void SetTextureVertWrapFuncInTarget(uint func, uint target, uint texUnitIndex)
        {

        }

        public void GenerateMipmapForTarget(uint target, uint texUnitIndex)
        {
        }

        #endregion Textures


        #region Matrices

        public void ActivateMatrixStack(int mode)
        {

        }

        public void LoadModelviewMatrix(LCC3Matrix4x3 matrix)
        {

        }

        public void LoadProjectionMatrix(LCC3Matrix4x4 matrix)
        {

        }

        public void PushModelviewMatrixStack()
        {

        }

        public void PopModelviewMatrixStack()
        {

        }

        public void PushProjectionMatrixStack()
        {

        }

        public void PopProjectionMatrixStack()
        {

        }

        #endregion Matrices


        #region Hints

        public void SetGenerateMipmapHint(uint hint)
        {

        }

        #endregion Hints


        #region Platform limits

        public uint MaxNumberOfLights()
        {
            return 0;
        }

        public uint MaxNumberOfClipPlanes()
        {
            return 0;
        }

        public uint MaxNumberOfPaletteMatrices()
        {
            return 0;
        }

        public uint MaxNumberOfTextureUnits()
        {
            return 0;
        }

        public uint MaxNumberOfVertexAttributes()
        {
            return 0;
        }

        public uint MaxNumberOfVertexUnits()
        {
            return 0;
        }

        public uint MaxNumberOfPixelSamples()
        {
            return 0;
        }

        #endregion Platform limits


        #region Shaders

        public void BindProgramWithVisitor(LCC3ShaderProgram program, LCC3NodeDrawingVisitor visitor)
        {

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

        public void align3DStateCache()
        {

        }

        #endregion Aligning 2D & 3D caches


        #region State management functions

        #endregion State management functions
    }
}

