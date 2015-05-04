using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextColorScroll : MonoBehaviour 
{
	[SerializeField] float _scrollTime = 4f;
	Text _text = null;

	void Awake()
	{
		_text = GetComponent<Text>();

		StartCoroutine( ColorScroll() );
	}

	IEnumerator ColorScroll()
	{
		while( true )
		{
			Color initColor = _text.color;
			float scrollTimer = 0f;

			while( scrollTimer < _scrollTime )
			{
				HSVColor hsvColor = WadeUtils.RGBToHSV( _text.color );
				hsvColor.h = WadeUtils.RGBToHSV( initColor ).h + scrollTimer/_scrollTime;

				if( hsvColor.h > 1f )
				{
					hsvColor.h -= 1f;
				}

				_text.color = WadeUtils.HSVToRGB( hsvColor );
				scrollTimer += Time.deltaTime;

				yield return 0;
			}
		}
	}
}
