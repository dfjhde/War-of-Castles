using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ModelBtn : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    public MeshRenderer meshRenderer;
    public int buildingCode;
    public Team team;
    private void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            MakeMapManager.inst.model.gameObject.SetActive(true);
            MakeMapManager.inst.model.ChangeModel(buildingCode, team);
            MakeMapManager.inst.model.isChoosingAModel = true;
        }

        //this.transform.localScale *= 2f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MakeMapManager.inst.model.isChoosingAModel = false;
        this.transform.localScale *= 0.5f;
    }
}
