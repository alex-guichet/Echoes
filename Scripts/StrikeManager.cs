using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeManager : MonoBehaviour
{
	[SerializeField] private List<Strike> strikeList;

	public static StrikeManager Instance;
	
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(Instance.gameObject);
		}

		Instance = this;
	}

    public void ActivateStrike()
    {
	    if (strikeList.Count > 1)
	    {
		    strikeList[0].Activate();
		    strikeList.RemoveAt(0);
	    }
        else
        {
	        strikeList[0].Activate();
	        strikeList.RemoveAt(0);
			PlayerTarget.Instance.ReceiveDamage(1000f);
		}
    }
}
