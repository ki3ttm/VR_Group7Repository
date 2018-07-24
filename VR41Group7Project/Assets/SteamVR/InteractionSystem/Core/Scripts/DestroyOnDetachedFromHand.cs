//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Destroys this object when it is detached form the hand
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class DestroyOnDetachedFromHand : MonoBehaviour
	{
		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
		{
			Destroy( gameObject );
		}
	}
}
