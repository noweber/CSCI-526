using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.TerrainPrototype.Source
{
    public static class CombatCalculator
    {
        public static bool DoesThisUnitSurviveAttacking(BaseUnit attacker, BaseUnit defender, bool isAttacker)
        {
            // The attack only retains their tile damage bonus when leaving a tile to attack.
            int attackerDamage = attacker.GetDamage();
            int attackerLife = attacker.GetLife(false);

            // The defender retains all bonuses for the tile they are on.
            int defenderDamage = defender.GetDamage();
            int defenderLife = defender.GetLife();

            // Calculate whether the attacker survives:
            if(isAttacker && attackerLife <= defenderDamage)
            {
                return false;
            }
            if(!isAttacker && defenderLife <= attackerDamage)
            {
                return false;
            }
            return true;
        }
    }
}
