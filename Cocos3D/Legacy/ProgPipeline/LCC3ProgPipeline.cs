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
        // Static vars

        public const LCC3VertexAttrIndex VertexAttributeIndexUnavailable = LCC3VertexAttrIndex.VertexAttribUnavailable;
        const int _defaultNumberOfBuffers = 5;
        static LCC3ProgPipeline _sharedProgPipeline;

        // ivars

        Game _xnaGame;
        GraphicsDevice _xnaGraphicsDevice;
        VertexBuffer _xnaVertexBuffer;
        IndexBuffer _xnaIndexBuffer;
        BlendState _xnaBlendState;
        CullMode _xnaCullMode;
        DepthStencilState _xnaDepthStencilState;
        float _xnaDepthBias;
        float _xnaSlopeScaleDepthBias;

        Color _xnaClearColor;
        float _xnaClearDepth;
        int _xnaClearStencil;

        int _xnaCurrentlyActiveTextureUnitIndex;

        LCC3ShaderProgram _currentlyActiveShader;
        LCC3VertexAttr[] _vertexAttributes;
        CC3VertexType[] _vertexData;
        CC3BufferAndTarget[] _buffers;

        Stack<LCC3Matrix4x4> _modelMatrixStack;
        Stack<LCC3Matrix4x4> _viewMatrixStack;
        Stack<LCC3Matrix4x4> _projMatrixStack;


        #region Properties

        public LCC3ShaderProgram CurrentlyActiveShader
        {
            get { return _currentlyActiveShader; }
            set { _currentlyActiveShader = value; }
        }

        public GraphicsDevice XnaGraphicsDevice
        {
            get { return _xnaGraphicsDevice; }
        }

        public Game XnaGame
        {
            get { return _xnaGame; }
        }

        // For testing only

        public VertexBuffer XnaVertexBuffer
        {
            get { return _xnaVertexBuffer; }
            set { _xnaVertexBuffer = value; _xnaGraphicsDevice.SetVertexBuffer(_xnaVertexBuffer); }
        }

        public IndexBuffer XnaIndexBuffer
        {
            get { return _xnaIndexBuffer; }
            set { _xnaIndexBuffer = value; _xnaGraphicsDevice.Indices = _xnaIndexBuffer; }
        }

        #endregion Properties


        #region Allocation and initialization

        public static LCC3ProgPipeline SharedPipeline(Game game)
        {
            if (_sharedProgPipeline == null)
            {
                _sharedProgPipeline = new LCC3ProgPipeline(game);
            }

            return _sharedProgPipeline;
        }

        public static LCC3ProgPipeline SharedPipeline()
        {
            return _sharedProgPipeline;
        }

        internal LCC3ProgPipeline(Game xnaGame)
        {
            CC3VertexType.DataSource = this;

            _xnaGame = xnaGame;
            _xnaGraphicsDevice = xnaGame.GraphicsDevice;
            _xnaBlendState = BlendState.Opaque;
            _xnaCullMode = CullMode.None;
            _xnaDepthStencilState = DepthStencilState.None;
            _xnaDepthBias = 0.0f;
            _xnaSlopeScaleDepthBias = 0.0f;

            _modelMatrixStack = new Stack<LCC3Matrix4x4>();
            _viewMatrixStack = new Stack<LCC3Matrix4x4>();
            _projMatrixStack = new Stack<LCC3Matrix4x4>();

            _buffers = new CC3BufferAndTarget[_defaultNumberOfBuffers];

            this.InitVertexAttributes();
            this.InitTextureUnits();
        }

        private void InitVertexAttributes()
        {
           // Array of vertex attributes based on number of attribute index enum types
           // There is a default "unavaible index" which is why we subtract the total num of enum items by 1
            _vertexAttributes = new LCC3VertexAttr[Enum.GetNames(typeof(LCC3VertexAttrIndex)).Length - 1];

            for (int i=0; i < _vertexAttributes.Length; i++)
            {
                _vertexAttributes[i] = new LCC3VertexAttr();
            }
        }

        private void InitTextureUnits()
        {
            _xnaCurrentlyActiveTextureUnitIndex = 0;
        }

        #endregion Allocation and initialization


        #region ICC3VertexTypeDataSource methods

        public bool VertexPositionEnabled()
        {
            return _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribPosition].IsEnabled;
        }

        public bool VertexTexCoordEnabled()
        {
            return _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribTexCoords].IsEnabled;
        }

        public bool VertexColorEnabled()
        {
            return _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribColor].IsEnabled;
        }

        public bool VertexNormalEnabled()
        {
            return _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribNormal].IsEnabled;
        }

        public List<LCC3VertexAttrIndex> EnabledVertexAttributeIndices()
        {
            List<LCC3VertexAttrIndex> vertexAttrIndices = new List<LCC3VertexAttrIndex>();

            if (this.VertexPositionEnabled())
            {
                vertexAttrIndices.Add(LCC3VertexAttrIndex.VertexAttribPosition);
            }

            if (this.VertexTexCoordEnabled())
            {
                vertexAttrIndices.Add(LCC3VertexAttrIndex.VertexAttribTexCoords);
            }

            if (this.VertexColorEnabled())
            {
                vertexAttrIndices.Add(LCC3VertexAttrIndex.VertexAttribColor);
            }

            if (this.VertexNormalEnabled())
            {
                vertexAttrIndices.Add(LCC3VertexAttrIndex.VertexAttribNormal);
            }

            return vertexAttrIndices;
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

            if (mesh.XnaVertexBuffer == null)
            {
                this.GenerateVertexBuffer();
                mesh.XnaVertexBuffer = _xnaVertexBuffer;
            }

            _xnaVertexBuffer = mesh.XnaVertexBuffer;
        }

        public void BindVertexAttributeWithVisitor(LCC3ShaderAttribute attribute, 
                                                   LCC3NodeDrawingVisitor visitor)
        {
            LCC3VertexArray vertexArray = this.VertexArrayForAttributeWithVisitor(attribute, visitor);
            vertexArray.BindContentToAttributeAtIndexWithVisitor(attribute.Location, visitor);
        }

        public void EnableVertexAttributeAtIndex(bool onOrOff, LCC3VertexAttrIndex vaIndex)
        {
            if (vaIndex >= 0)
            {
                LCC3VertexAttr vertexAttribute = _vertexAttributes[(int)vaIndex];

                vertexAttribute.IsEnabled = onOrOff;
                vertexAttribute.IsEnabledKnown = true;
            }
        }

        public LCC3VertexArray VertexArrayForAttributeWithVisitor(LCC3ShaderAttribute attribute, 
                                                                  LCC3NodeDrawingVisitor visitor)
        {
            return visitor.CurrentMesh.VertexArrayForSemanticAtIndex(attribute.Semantic, attribute.SemanticIndex);
        }

        public void BindVertexContentToAttributeAtIndex(object[] pData, 
                                                        LCC3ElementType elemType, 
                                                        bool shouldNormalize, 
                                                        LCC3VertexAttrIndex vaIndex)
        {
            if (vaIndex >= 0)
            {
                LCC3VertexAttr vertexAttribute = _vertexAttributes[(int)vaIndex];

                vertexAttribute.Vertices = pData;
                vertexAttribute.ElementType = elemType;
                vertexAttribute.ShouldNormalize = shouldNormalize;
                vertexAttribute.WasBound = true;
            }
        }

        private void GenerateVertexBuffer()
        {
            int numOfVertices = _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribPosition].Vertices.Length;
            _vertexData = new CC3VertexType[numOfVertices];

            for (int i=0; i < numOfVertices; i++)
            {
                object[] vtxData = _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribPosition].Vertices;
                LCC3Vector position = (LCC3Vector)(vtxData[i]);
                LCC3Vector normal = LCC3Vector.CC3VectorZero;
                CCTex2F texCoord = new CCTex2F(0.0f,0.0f);
                CCColor4B color = new CCColor4B(0, 0, 0, 0);

                if (this.VertexNormalEnabled())
                {
                    vtxData = _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribNormal].Vertices;
                    normal = (LCC3Vector)vtxData[i];
                }

                if (this.VertexTexCoordEnabled())
                {
                    vtxData = _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribTexCoords].Vertices;
                    texCoord = (CCTex2F)vtxData[i];
                }

                if (this.VertexColorEnabled())
                {
                    vtxData = _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribColor].Vertices;
                    color = (CCColor4B)vtxData[i];
                }

                _vertexData[i] = new CC3VertexType(position, normal, texCoord, color);
            }

            _xnaVertexBuffer = new VertexBuffer(_xnaGraphicsDevice, 
                                                typeof(CC3VertexType), 
                                                _vertexData.Length, 
                                                BufferUsage.None);

            _xnaVertexBuffer.SetData<CC3VertexType>( _vertexData );

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
            for (int i=0; i < _vertexAttributes.Length; i++)
            {
                this.EnableVertexAttributeAtIndex(_vertexAttributes[i].WasBound, (LCC3VertexAttrIndex)i);
            }
        }

        public void Enable2DVertexAttributes()
        {
            for (int i=0; i < _vertexAttributes.Length; i++)
            {
                this.EnableVertexAttributeAtIndex(false, (LCC3VertexAttrIndex)i);
            }

            _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribPosition].WasBound = true;
            _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribColor].WasBound = true;
            _vertexAttributes[(int)LCC3VertexAttrIndex.VertexAttribTexCoords].WasBound = true;
        }

        public uint GenerateBuffer()
        {
            int buffID = -1;
            int proposedBuffID = 0; 

            foreach (CC3BufferAndTarget buffAndTar in _buffers)
            {
                if (buffAndTar.Target == LCC3BufferTarget.None)
                {
                    buffID = proposedBuffID;
                    break;
                }

                proposedBuffID += 1;
            }

            if (buffID == -1)
            {
                Array.Resize(ref _buffers, _buffers.Length + 1);
                buffID = _buffers.Length;
            }

            return (uint)buffID;
        }

        public void DeleteBuffer(uint buffID)
        {
            CC3BufferAndTarget buffAndTarget = _buffers[(int)buffID];
            buffAndTarget.Buffer = null;
            buffAndTarget.Target = LCC3BufferTarget.None;
        }

        public void BindBufferToTarget(uint buffID, LCC3BufferTarget target)
        {
            CC3BufferAndTarget buffAndTarget = _buffers[(int)buffID];
            buffAndTarget.Target = target;
        }

        public void UnbindBuffer(uint buffID)
        {
            CC3BufferAndTarget buffAndTarget = _buffers[(int)buffID];
            buffAndTarget.Target = LCC3BufferTarget.None;
        }

        public void LoadBufferTarget(uint buffID, LCC3BufferTarget target, object[] vtxData)
        {
            CC3BufferAndTarget buffAndTarget = _buffers[(int)buffID];
            buffAndTarget.Buffer = vtxData;
        }

        public void UpdateBufferTarget(uint buffID, LCC3BufferTarget target, object[] vtxData, uint startingIndex)
        {
            CC3BufferAndTarget buffAndTarget = _buffers[(int)buffID];

            if (buffAndTarget.Buffer == null)
            {
                buffAndTarget.Buffer = vtxData;
            }
            else
            {
                object[] buffer = buffAndTarget.Buffer;

                Array.Resize(ref buffer, (int)startingIndex + vtxData.Length);
                Array.Copy(vtxData, 0, buffAndTarget.Buffer, (int)startingIndex, vtxData.Length);
            }
        }

        public void DrawVertices(LCC3DrawMode drawMode, int startIndex, int length)
        {
            _xnaGraphicsDevice.SetVertexBuffer(_xnaVertexBuffer);

            EffectPassCollection passes = _currentlyActiveShader.XnaShaderEffect.CurrentTechnique.Passes;

            for (int i=0; i < passes.Count; i++)
            {
                passes[i].Apply();
                _xnaGraphicsDevice.DrawPrimitives(drawMode.XnaPrimitiveType(), startIndex, length);
            }
        }

        public void DrawIndices<T>(uint vertexCount, uint startingVertexIndex, LCC3ElementType type, LCC3DrawMode drawMode, int minVertexIndex) where T : struct
        {
            _xnaGraphicsDevice.SetVertexBuffer(_xnaVertexBuffer);
            _xnaGraphicsDevice.Indices = _xnaIndexBuffer;

            foreach (EffectPass pass in _currentlyActiveShader.XnaShaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _xnaGraphicsDevice.DrawIndexedPrimitives(
                    drawMode.XnaPrimitiveType(), 
                    0, 
                    minVertexIndex, 
                    (int)vertexCount, 
                    (int)startingVertexIndex, 
                    (int)LCC3DrawableVertexArray.FaceCountFromVertexIndexCount((uint)vertexCount, drawMode));
            }
        }

        /*
        public void DrawIndices(LCC3DrawMode drawMode, int vertexOffset, int minVertexIndex, int numOfVertices, int startIndex, int primitiveCount)
        {
            foreach (EffectPass pass in _currentlyActiveShader.XnaShaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _xnaGraphicsDevice.DrawIndexedPrimitives(drawMode.XnaPrimitiveType(), vertexOffset, minVertexIndex, numOfVertices, startIndex, primitiveCount);
            }
        }
        */

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

        public void SetBlendFuncSrcAndDst(LCC3BlendType src, LCC3BlendType dst)
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

            Texture2D texture
                = new Texture2D(_xnaGraphicsDevice, width, height, false, textureFormat.XnaSurfaceFormat());

            texture.SetData<T>(imageData);

            _xnaGraphicsDevice.Textures[_xnaCurrentlyActiveTextureUnitIndex] = texture;
        }
       
        public void BindTextureToTargetAtIndex(LCC3GraphicsTexture texture, LCC3GraphicsTextureTarget target, uint texUnitIndex)
        {
            _xnaGraphicsDevice.Textures[(int)texUnitIndex] = texture.XnaTexture2D;

            this.ActivateTextureUnit(texUnitIndex);
        }

        public void ActivateTextureUnit(uint texUnitIndex)
        {
            _xnaCurrentlyActiveTextureUnitIndex = (int)texUnitIndex;
        }

        public void SetTextureMinifyFuncAtIndex(LCC3TextureFilter minifyFunction, uint texUnitIndex)
        {
            // MonoGame doesn't support MinFilter/MagFilter as per XNA spec
            _xnaGraphicsDevice.SamplerStates[(int)texUnitIndex].Filter = minifyFunction.XnaTextureFilter();
        }

        public void SetTextureMagnifyFuncAtIndex(LCC3TextureFilter magnifyFunction, uint texUnitIndex)
        {
            // MonoGame doesn't support MinFilter/MagFilter as per XNA spec
            _xnaGraphicsDevice.SamplerStates[(int)texUnitIndex].Filter = magnifyFunction.XnaTextureFilter();
        }

        public void SetTextureHorizWrapFuncAtIndex(LCC3TextureWrapMode wrapFunction, uint texUnitIndex)
        {
            // MonoGame doesn't support MinFilter/MagFilter as per XNA spec
            _xnaGraphicsDevice.SamplerStates[(int)texUnitIndex].AddressU = wrapFunction.XnaTextureAddressMode();
        }

        public void SetTextureVertWrapFuncAtIndex(LCC3TextureWrapMode wrapFunction, uint texUnitIndex)
        {
            // MonoGame doesn't support MinFilter/MagFilter as per XNA spec
            _xnaGraphicsDevice.SamplerStates[(int)texUnitIndex].AddressV = wrapFunction.XnaTextureAddressMode();
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

        public static uint MaxNumberOfLights
        {
            get { return 8; }
        }

        public static uint MaxNumberOfClipPlanes
        {
            get { return 6; }
        }

        public static uint MaxNumberOfPaletteMatrices
        {
            get { return 12; }
        }

        public static uint MaxNumberOfTextureUnits
        {
            get { return 5U; }
        }

        public static uint MaxNumberOfVertexAttributes
        {
            get { return 4; }
        }

        public static uint MaxNumberOfVertexUnits
        {
            get { return 4; }
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

            this.CurrentlyActiveShader = program;
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


    #region Private class holding buffer and target info

    internal class CC3BufferAndTarget
    {
        private object[] _buffer;
        private LCC3BufferTarget _target;

        public object[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        public LCC3BufferTarget Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public CC3BufferAndTarget(object[] buffer, LCC3BufferTarget target)
        {
            _buffer = buffer;
            _target = target;
        }

        public CC3BufferAndTarget(object[] buffer)
        {
            _buffer = buffer;
            _target = LCC3BufferTarget.None;
        }
    }

    #endregion Private struct holding buffer and target info


    #region Internal custom vertex declaration

    internal interface ICC3VertexTypeDataSource
    {
        bool VertexPositionEnabled();
        bool VertexTexCoordEnabled();
        bool VertexColorEnabled();
        bool VertexNormalEnabled();
    }

    internal struct CC3VertexType : IVertexType
    {
        private static ICC3VertexTypeDataSource _dataSource;

        private Vector3 _position;
        private Vector2 _texCoord;
        private Vector4 _color;
        private Vector3 _normal;

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

                vertexElements.Add(new VertexElement(currentOffset, VertexElementFormat.Vector3, VertexElementUsage.Position, 0));
                currentOffset += sizeof(float) * 3; 

                vertexElements.Add(new VertexElement(currentOffset, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));
                currentOffset += sizeof(float) * 2;

                vertexElements.Add(new VertexElement(currentOffset, VertexElementFormat.Vector4, VertexElementUsage.Color, 0));
                currentOffset += sizeof(float) * 4; 

                vertexElements.Add(new VertexElement(currentOffset, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0));
                currentOffset += sizeof(float) * 4; 

                VertexDeclaration vertexDec = new VertexDeclaration(vertexElements.ToArray());

                return vertexDec;
            }
        }

        public CC3VertexType(LCC3Vector position, LCC3Vector normal, CCTex2F texCoord, CCColor4B color)
        {
            _position = position.XnaVector;
            _texCoord = new Vector2(texCoord.U, texCoord.V);
            _color = LCC3ColorUtil.CCC4FFromCCC4B(color).ToVector4().XnaVector4;
            _normal = normal.XnaVector;
        }

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector3 Normal
        {
            get { return _normal; }
            set { _normal = value; }
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

    #endregion Internal custom vertex declaration
}

