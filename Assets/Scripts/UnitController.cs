using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private FlowFieldController _flowfieldController = null;
    [SerializeField]
    private GameObject _unitTemplate = null;
    private Stack<GameObject> _units = new Stack<GameObject>();

    private void AddUnit()
    {
        GameObject unitObj = Instantiate(_unitTemplate);
        Vector3 dimensions = _flowfieldController.Dimensions; //rows , cols , cellsize
        float x = Random.Range(0, dimensions.x * dimensions.z);
        float z = Random.Range(0, dimensions.y * dimensions.z);
        Vector3 newPos = new Vector3(x, 0, z);
        unitObj.transform.Translate(newPos);
        _units.Push(unitObj);
    }

    private void RemoveUnit()
    {
        if (_units.Count > 0)
            Destroy(_units.Pop());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            AddUnit();

        if (Input.GetKeyDown(KeyCode.Backspace))
            RemoveUnit();
    }

    private void FixedUpdate()
    {
        foreach (GameObject unit in _units)
        {
            FlowFieldCell cell = _flowfieldController.Grid.GetCellFromWorldPos(unit.transform.position);
            if (cell == null)
                continue;
            Vector3 newVel = new Vector3(cell.Direction.x, 0, cell.Direction.y);
            //unit.GetComponent<Rigidbody>().MovePosition(unit.transform.position + newVel * unit.GetComponent<FlowFieldUnit>().Speed * Time.deltaTime);
            unit.GetComponent<Rigidbody>().AddForce(newVel * unit.GetComponent<FlowFieldUnit>().Speed);
        }
    }
}