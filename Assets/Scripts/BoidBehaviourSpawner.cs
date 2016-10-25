﻿using UnityEngine;

using Utility;

public class BoidBehaviourSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Prefab;

    [SerializeField]
    private int m_NumberOfBoids;

    [SerializeField]
    private Vector3 m_MinPosition;
    [SerializeField]
    private Vector3 m_MaxPosition;

    // Use this for initialization
    void Start()
    {
        for (var i = 0; i < m_NumberOfBoids; ++i)
        {
            var newGameObject = Instantiate(m_Prefab);
            newGameObject.name = "Boid" + i;

            newGameObject.transform.SetParent(transform, false);

            var newBoid =
                BoidFactory.Create(
                    new Vector(m_MinPosition.x, m_MinPosition.y, m_MinPosition.z),
                    new Vector(m_MaxPosition.x, m_MaxPosition.y, m_MaxPosition.z));

            var newBoidBehaviour = newGameObject.AddComponent<BoidBehaviour>();
            newBoidBehaviour.Init(newBoid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Boid.tendToPosition =
            new Vector(
                mousePosition.x,
                mousePosition.y,
                mousePosition.z - Camera.main.transform.position.z);

        Boid.Update(Time.deltaTime);
    }
}
