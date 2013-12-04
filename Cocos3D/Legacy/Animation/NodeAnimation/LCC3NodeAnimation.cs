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


		#region Frame state

		private float TimeAtFrame(uint frameIndex) 
		{
			float currIdx = frameIndex;
			float lastIdx = _frameCount - 1;

			return MathHelper.Clamp(currIdx / lastIdx, 0.0f, 1.0f);
		}

		protected uint FrameIndexAtTime(float time) { return (uint)((_frameCount - 1) * time); }

		protected virtual LCC3Vector LocationAtFrame(uint frameIndex) { return LCC3Vector.CC3VectorZero; }

		protected virtual LCC3Quaternion QuaternionAtFrame(uint frameIndex) { return LCC3Quaternion.CC3QuaternionIdentity; }

		protected virtual LCC3Vector ScaleAtFrame(uint frameIndex) { return LCC3Vector.CC3VectorUnitCube; }

		#endregion Frame state


		#region Updating

		public void EstablishFrame(float time, LCC3NodeAnimationState animState)
		{
			uint frameIndex = this.FrameIndexAtTime(time);
			float frameInterpolation = 0.0f;

			if (_shouldInterpolate && (frameIndex < _frameCount - 1)) 
			{
				float frameTime = this.TimeAtFrame(frameIndex);
				float nextFrameTime = this.TimeAtFrame(frameIndex + 1);
				float frameDur = nextFrameTime - frameTime;

				if (frameDur != 0.0f)
				{
					frameInterpolation = (time - frameTime) / frameDur;
				}

				if (frameInterpolation < _interpolationEpsilon) 
				{
					frameInterpolation = 0.0f;
				} 

				else if ((1.0f - frameInterpolation) < _interpolationEpsilon) 
				{
					frameInterpolation = 0.0f;
					frameIndex++;					
				}
			}

			this.EstablishFrame(frameIndex, frameInterpolation, animState);
		}

		private void EstablishFrame(uint frameIndex, float frameInterpolation, LCC3NodeAnimationState animState)
		{
			this.EstablishLocationAtFrame(frameIndex, frameInterpolation, animState);
			this.EstablishQuaternionAtFrame(frameIndex, frameInterpolation, animState);
			this.EstablishScaleAtFrame(frameIndex, frameInterpolation, animState);
		}

		private void EstablishLocationAtFrame(uint frameIndex, float frameInterpolation, LCC3NodeAnimationState animState)
		{
			if(animState.IsAnimatingLocation) 
			{
				animState.Location = LCC3Vector.CC3VectorLerp(this.LocationAtFrame(frameIndex), this.LocationAtFrame(frameIndex + 1), frameInterpolation);
			}
		}

		private void EstablishQuaternionAtFrame(uint frameIndex, float frameInterpolation, LCC3NodeAnimationState animState)
		{
			if(animState.IsAnimatingQuaternion) 
			{
				animState.Quaternion = LCC3Quaternion.CC3QuaternionSlerp(this.QuaternionAtFrame(frameIndex), this.QuaternionAtFrame(frameIndex + 1), frameInterpolation);
			}
		}

		private void EstablishScaleAtFrame(uint frameIndex, float frameInterpolation, LCC3NodeAnimationState animState)
		{
			if(animState.IsAnimatingScale) 
			{
				animState.Scale = LCC3Vector.CC3VectorLerp(this.ScaleAtFrame(frameIndex), this.ScaleAtFrame(frameIndex + 1), frameInterpolation);
			}
		}

		#endregion Updating
    }
}

