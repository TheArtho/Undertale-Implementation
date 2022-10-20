using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EnemyAttack
{
       protected Enemy user;
       protected Battle battle;
       protected int damage;
       protected Vector2 areaSize = new Vector2(170, 140);     // new Vector2(170, 140);
       protected Vector2 areaPosition = new Vector2(0, -81);   // new Vector2(0, -81);

       public void Initialize(Battle battle, Enemy enemy)
       {
              this.battle = battle;
              this.user = enemy;
       }

       public virtual IEnumerator Use()
       {
              // Resize of the dodge area, place the soul at the center
              battle.scene.SetActiveSoul(true);
              battle.scene.ToggleBattleSoul(true);
              battle.scene.CenterSoul();
              battle.scene.StartCoroutine(battle.scene.SetBoxPosition(areaPosition, 0.5f));
              yield return battle.scene.StartCoroutine(battle.scene.SetBoxSize(areaSize, 0.5f));
              battle.scene.MoveSoul(true);
              
              // Dodge phase
              yield return battle.scene.StartCoroutine(this.Attack());
              
              battle.scene.ToggleBattleSoul(false);
              battle.scene.MoveSoul(false);
       }

       /// <summary>
       /// Attack coroutine to override
       /// </summary>
       /// <returns></returns>
       public virtual IEnumerator Attack()
       {
              // Default attack is doing nothing (waiting for 5 seconds)
              yield return new WaitForSeconds(5);
       }
       
       public override string ToString()
       {
              return "Default Attack";
       }
}

public class Vegetoid_Attack_01 : EnemyAttack
{
       public override string ToString()
       {
              return "Vegetoid Attack #1";
       }
       
       public override IEnumerator Attack()
       {
              // Instantiates falling carrots in the area
              List<GameObject> bullets = new List<GameObject>();
              Coroutine instantiation = battle.scene.StartCoroutine(Instantiation(bullets));
              
              yield return new WaitForSeconds(3);

              // Kills the carrots
              foreach (var b in bullets)
              {
                     GameObject.Destroy(b);
              }
              battle.scene.StopCoroutine(instantiation);
       }

       private IEnumerator Instantiation(List<GameObject> bullets)
       {
              float delay = 0.15f;
              GameObject prefab = Resources.Load<GameObject>("Prefabs/Falling Carrot");
              RectTransform box = battle.scene.battleBox.GetComponent<RectTransform>();

              while (true)
              {
                     GameObject bullet = GameObject.Instantiate(prefab, box.transform.Find("Mask/DodgeArea"));
                     bullet.GetComponent<RectTransform>().localPosition = 
                            new Vector3(
                                   UnityEngine.Random.Range(box.sizeDelta.x*-0.5f+13, box.sizeDelta.x*0.5f-13),
                                   box.sizeDelta.y*0.5f-5,
                                   0
                                   );
                     bullets.Add(bullet);
                     yield return new WaitForSeconds(delay);
              }
       }
}

public class Vegetoid_Attack_02 : EnemyAttack
{
       public override IEnumerator Attack()
       {
              // Instantiates bullets in the area
              List<GameObject> bullets = new List<GameObject>();
              Coroutine instantiation = battle.scene.StartCoroutine(Instantiation(bullets));
              
              yield return new WaitForSeconds(5);

              // Kills the bullets
              foreach (var b in bullets)
              {
                     GameObject.Destroy(b);
              }
              battle.scene.StopCoroutine(instantiation);
       }

       private IEnumerator Instantiation(List<GameObject> bullets)
       {
              float delay = 0.65f;
              GameObject prefab = Resources.Load<GameObject>("Prefabs/Bouncy Vegetable");
              RectTransform box = battle.scene.battleBox.GetComponent<RectTransform>();

              while (true)
              {
                     GameObject bullet = GameObject.Instantiate(prefab, box.transform.Find("Mask/DodgeArea"));
                     bullet.GetComponent<RectTransform>().localPosition = 
                            new Vector3(
                                   UnityEngine.Random.Range(box.sizeDelta.x*-0.5f+13, box.sizeDelta.x*0.5f-13),
                                   box.sizeDelta.y*0.5f-5,
                                   0
                            );
                     bullets.Add(bullet);
                     yield return new WaitForSeconds(delay);
              }
       }
       
       public override string ToString()
       {
              return "Vegetoid Attack #2";
       }
}