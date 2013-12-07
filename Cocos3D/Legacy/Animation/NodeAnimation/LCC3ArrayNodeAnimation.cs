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
	public class LCC3ArrayNodeAnimation : LCC3NodeAnimation
    {
		// ivars

		float[] _frameTimes;
		LCC3Vector[] _animatedLocations;
		LCC3Quaternion[] _animatedQuaternions;
		LCC3Vector[] _animatedScales;


		#region Properties

		// ivar properties

		public float[] FrameTimes
		{
			get { return _frameTimes; }
			set { _frameTimes = value; }
		}

		public LCC3Vector[] AnimatedLocations
		{
			get { return _animatedLocations; }
			set { _animatedLocations = value; }
		}

		public LCC3Quaternion[] AnimatedQuaternions
		{
			get { return _animatedQuaternions; }
			set { _animatedQuaternions = value; }
		}

		public LCC3Vector[] AnimatedScales
		{
			get { return _animatedScales; }
			set { _animatedScales = value; }
		}

		// animation state properties

		public override bool IsAnimatingLocation
		{
			get { return _animatedLocations != null; }
		}

		public override bool IsAnimatingQuaternion
		{
			get { return _animatedQuaternions != null; }
		}

		public override bool IsAnimatingScale
		{
			get { return _animatedScales != null; }
		}

		public override bool HasVariableFrameTiming
		{
			get { return _frameTimes != null; }
		}

		#endregion Properties


		#region Allocation and initialization

		public LCC3ArrayNodeAnimation(uint numOfFrames) : base(numOfFrames)
        {

        }

		public float[] AllocateFrameTimes()
		{
			if (this.FrameCount > 0) 
			{
				_frameTimes = new float[this.FrameCount];
			}

			return _frameTimes;
		}

		public LCC3Vector[] AllocateLocations()
		{
			if (this.FrameCount > 0) 
			{
				_animatedLocations = new LCC3Vector[this.FrameCount];
			}

			return _animatedLocations;
		}

		public LCC3Quaternion[] AllocateQuaternions()
		{
			if (this.FrameCount > 0) 
			{
				_animatedQuaternions = new LCC3Quaternion[this.FrameCount];
			}

			return _animatedQuaternions;
		}

		public LCC3Vector[] AllocateScales()
		{
			if (this.FrameCount > 0) 
			{
				_animatedScales = new LCC3Vector[this.FrameCount];
			}

			return _animatedScales;
		}

		#endregion Allocation and initialization


		#region Frame state

		protected override float TimeAtFrame(uint frameIndex) 
		{
			if (_frameTimes != null) { return base.TimeAtFrame(frameIndex);  }

			return _frameTimes[Math.Min(frameIndex, this.FrameCount - 1)];
		}

		protected override uint FrameIndexAtTime(float time)
		{
			if (_frameTimes != null) { return base.FrameIndexAtTime(time); }

			for (uint fIdx = this.FrameCount - 1; fIdx >= 0; fIdx--)
			{
				if (_frameTimes[fIdx] <= time)
				{
					return fIdx;
				}
			}

			return 0;
		}

		protected override LCC3Vector LocationAtFrame(uint frameIndex)
		{
			if (_animatedLocations != null) { return base.LocationAtFrame(frameIndex); }

			return _animatedLocations[Math.Min(frameIndex, this.FrameCount - 1)];
		}

		protected override LCC3Quaternion QuaternionAtFrame(uint frameIndex)
		{
			if (_animatedQuaternions != null) { return base.QuaternionAtFrame(frameIndex); }

			return _animatedQuaternions[Math.Min(frameIndex, this.FrameCount - 1)];
		}

		protected override LCC3Vector ScaleAtFrame(uint frameIndex)
		{
			if (_animatedScales != null) { return base.ScaleAtFrame(frameIndex); }

			return _animatedScales[Math.Min(frameIndex, this.FrameCount - 1)];
		}

		#endregion Frame state
    }

}

