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
    public struct CC3Matrix
    {
        // Static fields

        private static readonly CC3Matrix _identity = new CC3Matrix(Matrix.Identity);

        // Instance fields

        private Matrix _xnaMatrix;


        #region Properties

        // Static properties

        public static CC3Matrix CC3MatrixIdentity
        {
            get { return _identity; }
        }

        // Instance properties

        public /* internal */ Matrix XnaMatrix
        {
            get { return _xnaMatrix; }
        }

        #endregion Properties


        #region Static matrix construction methods

        public static CC3Matrix CreateWorldMatrix(CC3Vector worldPosition, 
                                                  CC3Vector worldScale,
                                                  CC3Quaternion localRotation,
                                                  CC3Quaternion rotationRelativeToAnchor,
                                                  CC3Vector anchorPointRelativeToPosition)
        {
            Matrix xnaTranslationMatrix = Matrix.CreateTranslation(worldPosition.XnaVector);
            Matrix xnaScaleMatrix = Matrix.CreateScale(worldScale.XnaVector);
            Matrix xnaLocalRotationMatrix = Matrix.CreateFromQuaternion(localRotation.XnaQuaternion);
            Matrix xnaRotationRelativeToAnchorMatrix = Matrix.CreateFromQuaternion(rotationRelativeToAnchor.XnaQuaternion);
            Matrix xnaRotationAnchorTranslationMatrix = Matrix.CreateTranslation(anchorPointRelativeToPosition.XnaVector);

            return new CC3Matrix(xnaTranslationMatrix *
                                 Matrix.Invert(xnaRotationAnchorTranslationMatrix) *
                                 xnaRotationRelativeToAnchorMatrix * 
                                 xnaRotationAnchorTranslationMatrix * 
                                 xnaLocalRotationMatrix *
                                 xnaScaleMatrix);
        }

        public static CC3Matrix CreateCameraViewMatrix(CC3Vector cameraPosition, 
                                                       CC3Vector cameraTarget,
                                                       CC3Quaternion cameraRotationRelativeToTarget)
        {
            Vector3 xnaCameraPosAfterRotation 
                = Vector3.Transform(cameraPosition.XnaVector - cameraTarget.XnaVector, 
                                    cameraRotationRelativeToTarget.XnaQuaternion);

            xnaCameraPosAfterRotation += cameraTarget.XnaVector;

            Matrix xnaViewMatrix = Matrix.CreateLookAt(xnaCameraPosAfterRotation,
                                                       cameraTarget.XnaVector, 
                                                       CC3Vector.CC3VectorUp.XnaVector);
            
            return new CC3Matrix(xnaViewMatrix);
        }
       
        #endregion Static camera view calculation methods


        #region Constructors

        internal CC3Matrix(Matrix xnaMatrix)
        {
            // Structs copy by value
            _xnaMatrix = xnaMatrix;
        }

        #endregion Constructors

        #region Calculation methods

        public CC3Matrix Inverse()
        {
            return new CC3Matrix(Matrix.Invert(_xnaMatrix));
        }

        public CC3Vector TranslationOfTransformMatrix()
        {
            return new CC3Vector(_xnaMatrix.Translation);
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

        #endregion Calculation methods
    }
}

