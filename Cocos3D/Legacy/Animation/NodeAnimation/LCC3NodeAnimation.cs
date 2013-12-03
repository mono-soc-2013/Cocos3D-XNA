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

namespace Cocos3D
{
	public abstract class LCC3NodeAnimation
    {
		// static vars

		static float _interpolationEpsilon = 0.1f;

		// ivars

		uint _frameCount;
		bool _shouldInterpolate; 


		#region Properties

		// static properties

		public static float InterpolationEpsilon
		{
			get { return _interpolationEpsilon; }
			set { _interpolationEpsilon = value; }
		}

		// ivar properties

		public uint FrameCount
		{
			get { return _frameCount; }
		}

		public bool ShouldInterpolate 
		{
			get { return _shouldInterpolate; }
			set { _shouldInterpolate = value; }
		}

		// animation state properties

		public virtual bool IsAnimatingLocation
		{
			get { return false; }
		}

		public virtual bool IsAnimatingQuaternion
		{
			get { return false; }
		}

		public virtual bool IsAnimatingScale
		{
			get { return false; }
		}

		public bool IsAnimating
		{
			get { return (this.IsAnimatingLocation || this.IsAnimatingQuaternion || this.IsAnimatingScale); }
		}

		public virtual bool HasVariableFrameTiming
		{
			get { return false; }
		}

		#endregion Properties


		#region Allocation and initialization

		public LCC3NodeAnimation(uint numOfFrames)
        {
			_frameCount = numOfFrames;
			_shouldInterpolate = true;
        }

		#endregion Allocation and initialization
    }
}

