using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject enemyPrefab;

    public Transform[] spawnPoints;

    public float spawnRange = 2;

    public void SpawnRobot(float hp, float damage, float speed)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        Vector3 spawnPos = spawnPoints[spawnIndex].position;

        //Add a random position 
        spawnPos.x += Random.Range(-spawnRange, spawnRange);
        spawnPos.z += Random.Range(-spawnRange, spawnRange);

        Robots robot = Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation).GetComponent<Robots>();
        robot.health = hp;
        robot.damage = damage;
        robot.speed = speed;
    }

    /// <summary>
    /// Wait until all robots are dead
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForAllRobotsToBeDead()
    {
        //Wait for all robots to be dead to finish
        Robots[] robots2 = FindObjectsOfType<Robots>();
        int aliveRobots2 = 0;
        foreach (Robots robot in robots2)
        {
            if (!robot.isDead)
                aliveRobots2++;
        }
        while (aliveRobots2 > 0)
        {
            yield return new WaitForSeconds(0.5f);
            robots2 = FindObjectsOfType<Robots>();
            aliveRobots2 = 0;
            foreach (Robots robot in robots2)
            {
                if (!robot.isDead)
                    aliveRobots2++;
            }
        }
    }

    public IEnumerator SpawnWave(int robotsNumber, int simultaneousNumber, float hp, float damage, float speed)
    {
        //Spawn 10 robots, 3 in simultaneous
        for (int i = 0; i < robotsNumber; i++)
        {
            SpawnRobot(hp, damage, speed);
            //Get all existing robots not dead
            Robots[] robots = FindObjectsOfType<Robots>();
            int aliveRobots = 0;
            foreach (Robots robot in robots)
            {
                if (!robot.isDead)
                    aliveRobots++;
            }
            //Wait until there are 3 alive robots
            while (aliveRobots < simultaneousNumber)
            {
                yield return new WaitForSeconds(0.5f);
                robots = FindObjectsOfType<Robots>();
                aliveRobots = 0;
                foreach (Robots robot in robots)
                {
                    if (!robot.isDead)
                        aliveRobots++;
                }
            }
            //Wait 1 second
            yield return new WaitForSeconds(1);
        }

    }

    public IEnumerator StartBeginLevel()
    {
        //Spawn 10 robots, 3 in simultaneous
        yield return StartCoroutine(SpawnWave(10, 3, 1, 1, 1));

        //Wait for all robots to be dead to finish
        yield return StartCoroutine(WaitForAllRobotsToBeDead());

    }

    public IEnumerator StartIntermediaireLevel()
    {
        yield return StartCoroutine(SpawnWave(20, 5, 5, 1, 1.1f));

        //Wait for all robots to be dead to finish
        yield return StartCoroutine(WaitForAllRobotsToBeDead());

    }

    

    public IEnumerator StartAdvancedLevel()
    {
        //A lot of robots
        yield return StartCoroutine(SpawnWave(50, 25, 3, 1, 2));

        //Strong one
        yield return StartCoroutine(SpawnWave(10, 5, 30, 3, 0.75f));

        //Wait for all robots to be dead to finish
        yield return StartCoroutine(WaitForAllRobotsToBeDead());

    }

    public IEnumerator StartImpossibleLevel()
    {
        //A lot of robots
        yield return StartCoroutine(SpawnWave(100, 50, 5, 1, 2));

        //Strong one
        yield return StartCoroutine(SpawnWave(25, 10, 50, 5, 1.2f));

        //Wait for all robots to be dead to finish
        yield return StartCoroutine(WaitForAllRobotsToBeDead());

    }


    public void StartLevel(int level)
    {
        //Reset and kill all mobs
        StopAllCoroutines();
        Robots[] robots = FindObjectsOfType<Robots>();
        foreach (Robots robot in robots)
        {
            Destroy(robot.gameObject);
        }

        switch (level)
        {
            case 0:
                StartCoroutine(StartBeginLevel());
                break;
            case 1:
                StartCoroutine(StartIntermediaireLevel());
                break;
            case 2:
                StartCoroutine(StartAdvancedLevel());
                break;
            case 3:
                StartCoroutine(StartImpossibleLevel());
                break;
        }
        
    }


}
