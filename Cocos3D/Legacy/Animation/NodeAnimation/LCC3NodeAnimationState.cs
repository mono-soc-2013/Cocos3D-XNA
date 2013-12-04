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
    public class LCC3NodeAnimationState
    {
		// static vars

		static uint _lastTrackID = 0;

		// ivars

		bool _isEnabled;
		bool _isLocationAnimationEnabled;
		bool _isQuaternionAnimationEnabled;
		bool _isScaleAnimationEnabled;

		uint _trackID;

		float _blendingWeight;
		float _animationTime;

		LCC3Vector _location;
		LCC3Quaternion _quaternion;
		LCC3Vector _scale;

		LCC3Node _node;
		LCC3NodeAnimation _animation;


		#region Properties

		public LCC3Node Node
		{
			get { return _node; }
		}

		public LCC3NodeAnimation Animation
		{
			get { return _animation; }
		}

		public uint TrackID
		{
			get { return _trackID; }
		}

		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { _isEnabled = value; this.MarkDirty(); }
		}

		public float BlendingWeight
		{
			get { return _blendingWeight; }
			set { _blendingWeight = MathHelper.Clamp(value, 0.0f, 1.0f); this.MarkDirty(); }  
		}

		public LCC3Vector Location
		{
			get { return _location; }
			set 
			{
				_location = value;
				this.MarkDirty();
			} 
		}

		public LCC3Quaternion Quaternion
		{
			get { return _quaternion; }
			set 
			{
				_quaternion = value;
				this.MarkDirty();
			}
		}

		public LCC3Vector Scale
		{
			get { return _scale; }
			set
			{
				_scale = value;
				this.MarkDirty();
			}
		}

		public uint FrameCount
		{
			get { return _animation.FrameCount; }
		}

		public bool IsAnimatingLocation
		{
			get { return (_isLocationAnimationEnabled && _animation.IsAnimatingLocation); }
		}

		public bool IsAnimatingQuaternion
		{
			get { return (_isQuaternionAnimationEnabled && _animation.IsAnimatingQuaternion); }
		}

		public bool IsAnimatingScale
		{
			get { return (_isScaleAnimationEnabled && _animation.IsAnimatingScale); }
		}

		public bool IsAnimating
		{
			get { return (this.IsEnabled && (this.IsAnimatingLocation || this.IsAnimatingQuaternion || this.IsAnimatingScale)); } 
		}

		public bool HasVariableFrameTiming
		{
			get { return _animation.HasVariableFrameTiming; }
		}

		#endregion Properties


		#region Allocation and initialization

		public static uint GenerateTrackID() { return ++_lastTrackID; }


		public LCC3NodeAnimationState(LCC3NodeAnimation animation, uint trackID, LCC3Node node)
        {
			_node = node;					
			_animation = animation;
			_trackID = trackID;
			_blendingWeight = 1.0f;
			_animationTime = 0.0f;
			_location = LCC3Vector.CC3VectorZero;
			_quaternion = LCC3Quaternion.CC3QuaternionIdentity;
			_scale = LCC3Vector.CC3VectorUnitCube;
			_isEnabled = true;
			_isLocationAnimationEnabled = true;
			_isQuaternionAnimationEnabled = true;
			_isScaleAnimationEnabled = true;

			this.EstablishFrame(0.0f);
        }

		#endregion Allocation and initialization


		#region Updating

		public void EstablishFrame(float time)
		{
			_animationTime = time;
			if (this.IsEnabled)
			{
				_animation.EstablishFrame(time, this);
			}
		}

		private void MarkDirty()
		{
			_node.MarkTransformDirty();
		}

		#endregion Updating
    }
}

