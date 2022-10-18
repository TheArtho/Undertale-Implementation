using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAttack
{
       protected Enemy user;
       protected Battle battle;
       protected int damage;
       protected Vector2 areaSize = new Vector2(170, 140);
       protected Vector2 areaPosition = new Vector2(0, -81);

       public void Initialize(Battle battle, Enemy enemy)
       {
              this.battle = battle;
              this.user = enemy;
       }

       public virtual IEnumerator Use()
       {
              // Resize of the dodge area
              battle.scene.SetActiveSoul(true);
              battle.scene.CenterSoul();
              battle.scene.StartCoroutine(battle.scene.SetBoxPosition(areaPosition, 0.5f));
              yield return battle.scene.StartCoroutine(battle.scene.SetBoxSize(areaSize, 0.5f));
              
              // Dodge phase
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
}

public class Vegetoid_Attack_02 : EnemyAttack
{
       public override string ToString()
       {
              return "Vegetoid Attack #2";
       }
}