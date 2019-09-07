using UnityEngine;
using Valve.VR;

public class Draggable : MonoBehaviour
{
	public bool fixX;
	public bool fixY;
	public Transform thumb;
    public Transform minBound;
	bool dragging;

    public SteamVR_Input_Sources controller =SteamVR_Input_Sources.RightHand;
    public SteamVR_Behaviour_Pose controllerPose;


    /// <summary>
    /// In case the right hand's controller's trigger is pressed, cast a forward ray and drag the selector
    /// </summary>
    void FixedUpdate()
	{

        //Wenn trigger gedrückt wird
        if (SteamVR_Actions.default_Squeeze.GetAxis(SteamVR_Input_Sources.RightHand) >= 0.5f) {
			dragging = false;

            Ray ray = new Ray(controllerPose.transform.position, controllerPose.transform.forward);
            
            RaycastHit hit;
			if (GetComponent<Collider>().Raycast(ray, out hit, 100)) {
				dragging = true;
			}
		}
		if (SteamVR_Actions.default_Squeeze.GetAxis(SteamVR_Input_Sources.RightHand) < 0.5f) dragging = false;
		if (dragging && (SteamVR_Actions.default_Squeeze.GetAxis(SteamVR_Input_Sources.RightHand) >= 0.5f)) {
            Ray ray = new Ray(controllerPose.transform.position, controllerPose.transform.forward);
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (GetComponent<Collider>().Raycast(ray, out hit, 100))
            {
                var point = hit.point;
                //point = GetComponent<Collider>().ClosestPointOnBounds(point);
                SetThumbPosition(point);
                SendMessage("OnDrag", Vector3.one - (thumb.localPosition - minBound.localPosition) / GetComponent<BoxCollider>().size.x);
            }
		}
	}

	void SetDragPoint(Vector3 point)
	{
		point = (Vector3.one - point) * GetComponent<Collider>().bounds.size.x + GetComponent<Collider>().bounds.min;
		SetThumbPosition(point);
	}

	void SetThumbPosition(Vector3 point)
	{
        Vector3 temp = thumb.localPosition;
        thumb.position = point;
        thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? temp.y : thumb.localPosition.y, thumb.localPosition.z - .01f);
	}
}
