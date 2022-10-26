using UnityEngine;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
	[Range(-0.1f, 0.1f)]
	public float curveStrength = 0.01f;
	private bool _isIncreaseCurve = true;

    int m_CurveStrengthID;

    private void OnEnable()
    {
        m_CurveStrengthID = Shader.PropertyToID("_CurveStrength");
    }

	void Update()
	{
		Shader.SetGlobalFloat(m_CurveStrengthID, curveStrength);
		Invoke("CurveMod", 2.5f);
		
	}

	public void CurveMod() // limitando a curva e aumentando/diminuindo
	{
		if(_isIncreaseCurve == true)
		{
			curveStrength += 0.0005f * Time.deltaTime;
		}
		else
		{
			curveStrength -= 0.0005f * Time.deltaTime;
		}
		if(curveStrength >= 0.01f)
		{
			_isIncreaseCurve = false;
		}
		else if(curveStrength <= -0.01f)
		{
			_isIncreaseCurve = true;
		}
	}

}
