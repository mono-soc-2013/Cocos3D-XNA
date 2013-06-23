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

namespace Cocos3D
{
    public class CC3CameraOrthographicBuilder : CC3CameraBuilder
    {
        // Instance fields

        private float _cameraViewWidth;
        private float _cameraViewHeight;


        #region Constructors

        public CC3CameraOrthographicBuilder() : base()
        {

        }

        #endregion Constructors


        #region Building camera methods

        public override CC3Camera Build()
        {
            CC3CameraOrthographic camera 
                = new CC3CameraOrthographic(_cameraPostion, _cameraTarget, 
                                            _cameraViewWidth, _cameraViewHeight, 
                                            _cameraNearClippingDistance, _cameraFarClippingDistance);

            return camera;
        }

        #endregion Building camera methods


        #region Setting up camera projection matrix

        public CC3CameraOrthographicBuilder WithViewWidth(float cameraViewWidth)
        {
            _cameraViewWidth = cameraViewWidth;

            return this;
        }

        public CC3CameraOrthographicBuilder WithViewHeight(float cameraViewHeight)
        {
            _cameraViewHeight = cameraViewHeight;

            return this;
        }
       
        #endregion Setting up camera projection matrix
    }
}

