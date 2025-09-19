using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegControls : MonoBehaviour
{
    public Transform leftTarget; //IK target for left leg
    public Transform rightTarget; //IK target for right leg
    public Transform Lray; //raycast to determine where left leg will go
    public Transform Rray;//raycast to determine where right leg will go
    public Transform Hips; //hips/chest

    public float LegStepUp; //the height/level the leg will go prior to stepping

    private Vector3 ShouldBeL;//where IK target will try to lerp to
    private Vector3 ShouldBeR;//where IK target will try to lerp to

    private Vector3 ShouldReallyBeL;//where the floot will end up if stepped
    private Vector3 ShouldReallyBeR;//where the floot will end up if stepped

    public float DistL;//distance from left foot to ShouldReallyBeL
    public float DistR;//distance from left foot to ShouldReallyBeR

    private bool LStepping;//true when left leg stepped
    private bool RStepping;//true when right leg stepped

    private string LastStep;//tracks last foot stepped

    public float wantStepAt;//distence that you will step at

    public float legSpeed;//speed of the steps


    // Start is called before the first frame update
    void Start()
    {
        ShouldBeL = leftTarget.position;
        ShouldBeR = rightTarget.position;
        LastStep = "R";
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitL;
        if (Physics.Raycast(Lray.position, Vector3.down, out hitL))
        {
            ShouldBeL = new Vector3(hitL.point.x, hitL.point.y, hitL.point.z);
        }
        RaycastHit hitR;
        if (Physics.Raycast(Rray.position, Vector3.down, out hitR))
        {
            ShouldBeR = new Vector3(hitR.point.x, hitR.point.y, hitR.point.z);
        }

        //distance for the feet
        DistL = Vector3.Distance(ShouldBeL, ShouldReallyBeL);
        DistR = Vector3.Distance(ShouldBeR, ShouldReallyBeR);

        //to make the stepping smooth
        leftTarget.position = Vector3.Lerp(leftTarget.position, ShouldBeL, legSpeed + Time.deltaTime);
        rightTarget.position = Vector3.Lerp(rightTarget.position, ShouldBeR, legSpeed + Time.deltaTime);

        //figure out which foot should take a step
        if (!LStepping && !RStepping)
        {
            if (LastStep == "R")
            {
                if (DistL > wantStepAt)
                {
                    StartCoroutine(stepL());
                    LastStep = "L";
                    Debug.Log("Left");
                }
            }
            else if (LastStep == "L")
            {
                if (DistR > wantStepAt)
                {
                    StartCoroutine(stepR());
                    LastStep = "R";
                    Debug.Log("Right");
                }
            }
        }

        //make leg y rotation look normal
        leftTarget.transform.eulerAngles = new Vector3(0, Hips.eulerAngles.y, 0);
        rightTarget.transform.eulerAngles = new Vector3(0, Hips.eulerAngles.y, 0);

    }

    IEnumerator stepL()
    { 
        LStepping = true;//tell script we are stepping
        ShouldBeL = new Vector3(ShouldBeL.x, ShouldReallyBeL.y + LegStepUp, ShouldBeL.z);//make foot go up
        yield return new WaitForSeconds(0.3f);
        ShouldBeL = ShouldReallyBeL * 1f; //places foot in correct position
        yield return new WaitForSeconds(0.2f);//prevents it from stepping immediatelty after
        LStepping = false;//tells script that we have finished stepping
    }
    IEnumerator stepR()
    {
        RStepping = true;//tell script we are stepping
        ShouldBeR = new Vector3(ShouldBeR.x, ShouldReallyBeR.y + LegStepUp, ShouldBeR.z);//make foot go up
        yield return new WaitForSeconds(0.3f);
        ShouldBeR = ShouldReallyBeR * 1f; //places foot in correct position
        yield return new WaitForSeconds(0.2f);//prevents it from stepping immediatelty after
        RStepping = false;//tells script that we have finished stepping
    }
}
