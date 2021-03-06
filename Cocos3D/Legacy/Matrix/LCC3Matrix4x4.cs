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
using Microsoft.Xna.Framework;

namespace Cocos3D
{
    public class LCC3Matrix4x4
    {
        // Static vars

        static readonly LCC3Matrix4x4 _identity = new LCC3Matrix4x4(Matrix.Identity);

        // ivars

        Matrix _xnaMatrix;


        #region Properties

        // Static properties

        public static LCC3Matrix4x4 CC3MatrixIdentity
        {
            get { return _identity; }
        }

        // Instance properties

        public /* internal */ Matrix XnaMatrix
        {
            get { return _xnaMatrix; }
        }

        #endregion Properties


        #region Operators

        public static LCC3Matrix4x4 operator *(LCC3Matrix4x4 matrix1, LCC3Matrix4x4 matrix2)
        {
            return new LCC3Matrix4x4(matrix1.XnaMatrix * matrix2.XnaMatrix);
        }

        #endregion Operators


        #region Static matrix construction methods

        public static LCC3Matrix4x4 CreateWorldMatrix(LCC3Vector worldPosition, 
                                                      LCC3Vector worldScale,
                                                      CC3Quaternion localRotation,
                                                      CC3Quaternion rotationRelativeToAnchor,
                                                      LCC3Vector anchorPointRelativeToPosition)
        {
            Matrix xnaTranslationMatrix = Matrix.CreateTranslation(worldPosition.XnaVector);
            Matrix xnaScaleMatrix = Matrix.CreateScale(worldScale.XnaVector);
            Matrix xnaLocalRotationMatrix = Matrix.CreateFromQuaternion(localRotation.XnaQuaternion);
            Matrix xnaRotationRelativeToAnchorMatrix = Matrix.CreateFromQuaternion(rotationRelativeToAnchor.XnaQuaternion);
            Matrix xnaRotationAnchorTranslationMatrix = Matrix.CreateTranslation(anchorPointRelativeToPosition.XnaVector);

            return new LCC3Matrix4x4(xnaTranslationMatrix * 
                                     Matrix.Invert(xnaRotationAnchorTranslationMatrix) *
                                     xnaRotationRelativeToAnchorMatrix * 
                                     xnaRotationAnchorTranslationMatrix * 
                                     xnaLocalRotationMatrix *
                                     xnaScaleMatrix);
        }

        public static LCC3Matrix4x4 CreateTranslationMatrix(LCC3Vector worldTranslation)
        {
            Matrix xnaTranslationMatrix = Matrix.CreateTranslation(worldTranslation.XnaVector);

            return new LCC3Matrix4x4(xnaTranslationMatrix);
        }

        public static LCC3Matrix4x4 CreateCameraViewMatrix(LCC3Vector cameraPosition, 
                                                           LCC3Vector cameraTarget,
                                                           CC3Quaternion cameraRotationRelativeToTarget,
                                                           LCC3Vector cameraUpDirection)
        {
            Vector3 xnaCameraPosAfterRotation 
                = Vector3.Transform(cameraPosition.XnaVector - cameraTarget.XnaVector, 
                                    cameraRotationRelativeToTarget.XnaQuaternion);

            xnaCameraPosAfterRotation += cameraTarget.XnaVector;

            Matrix xnaViewMatrix = Matrix.CreateLookAt(xnaCameraPosAfterRotation,
                                                       cameraTarget.XnaVector, 
                                                       cameraUpDirection.XnaVector);

            return new LCC3Matrix4x4(xnaViewMatrix);
        }

        #endregion Static camera view calculation methods


        #region Constructors

        public LCC3Matrix4x4(Matrix xnaMatrix)
        {
            _xnaMatrix = xnaMatrix;
        }

        #endregion Constructors


        #region Calculation methods

        public LCC3Matrix4x4 Inverse()
        {
            return new LCC3Matrix4x4(Matrix.Invert(_xnaMatrix));
        }

        public LCC3Vector TranslationOfTransformMatrix()
        {
            return new LCC3Vector(_xnaMatrix.Translation);
        }

        public CC3Quaternion LocalRotationOfTransformMatrix()
        {
            CC3Quaternion localRotation = CC3Quaternion.CC3QuaternionIdentity;
            Vector3 xnaTranslation;
            Vector3 xnaScale;
            Quaternion xnaRotation;

            if (this.XnaMatrix.Decompose(out xnaScale, out xnaRotation, out xnaTranslation) == true)
            {
                localRotation = new CC3Quaternion(xnaRotation);
            }

            return localRotation;
        }

        public LCC3Vector4 TransformCC3Vector4(LCC3Vector4 vec4)
        {
            return new LCC3Vector4(Vector4.Transform(vec4.XnaVector4, _xnaMatrix));
        }

        public LCC3Matrix4x4 Transpose()
        {
            return new LCC3Matrix4x4(Matrix.Transpose(_xnaMatrix));
        }

        public LCC3Matrix4x4 InverseAdjoint()
        {
            LCC3Matrix4x4 inverse = this.Inverse();
            Matrix xnaInvs = inverse.XnaMatrix;
            float determinant = xnaInvs.Determinant();

            if (determinant != 0.0f)
            {
                xnaInvs /= determinant;
            }

            return new LCC3Matrix4x4(xnaInvs);
        }

        public LCC3Matrix4x4 InverseAdjointTranspose()
        {
            return this.InverseAdjoint().Transpose();
        }

        #endregion Calculation methods
    }
}

