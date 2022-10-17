public class BattleChoice
{
        /// <summary>
        /// Battle Command chosen
        /// </summary>
        public Battle.BattleCommand command { get; protected set; }
        /// <summary>
        /// Target for the command (based on the opponents array order)
        /// </summary>
        public int target { get; private set; }
        
        public BattleChoice(int target)
        {
                this.target = target;
        }

        public BattleChoice(Battle.BattleCommand command, int target)
        {
                this.command = command;
                this.target = target;
        }
}

public class BattleFightChoice : BattleChoice
{
        public BattleFightChoice(int target) : base(target)
        {
                command = Battle.BattleCommand.FIGHT;
        }
}

public class BattleActChoice : BattleChoice
{
        public BattleActChoice(int target) : base(target)
        {
                command = Battle.BattleCommand.ACT;
        }
}

public class BattleItemChoice : BattleChoice
{
        public BattleItemChoice(int target) : base(target)
        {
                command = Battle.BattleCommand.ITEM;
        }
}

public class BattleMercyChoice : BattleChoice
{
        public BattleMercyChoice(int target) : base(target)
        {
                command = Battle.BattleCommand.MERCY;
        }
}