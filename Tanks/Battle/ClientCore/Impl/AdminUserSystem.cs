namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class AdminUserSystem : ElevatedAccessUserBaseSystem
    {
        [OnEventFire]
        public void InitAdminConsole(NodeAddedEvent e, AdminUserNode admin)
        {
            base.InitConsole(admin);
            SmartConsole.RegisterCommand("exception", "Throws NullReferenceException", new SmartConsole.ConsoleCommandFunction(this.ThrowNullPointer));
            SmartConsole.RegisterCommand("dropSupply", "dropSupply ARMOR", "Drops Supply in Current", new SmartConsole.ConsoleCommandFunction(this.DropSupply));
            SmartConsole.RegisterCommand("dropGold", "dropGold CRY", "Drops Gold in Current", new SmartConsole.ConsoleCommandFunction(this.DropGold));
            SmartConsole.RegisterCommand("blockUser", "blockUser User1 CHEATING", "Blocks user. Possible reasons: RULES_ABUSE, SABOTAGE, CHEATING, FRAUD...", new SmartConsole.ConsoleCommandFunction(this.BlockUser));
            SmartConsole.RegisterCommand("runCommand", "Run server console command", new SmartConsole.ConsoleCommandFunction(this.RunCommand));
            SmartConsole.RegisterCommand("createUserItem", "createUserItem -1816745725 3", "Create user item", new SmartConsole.ConsoleCommandFunction(this.CreateUserItem));
            SmartConsole.RegisterCommand("wipeUserItems", "wipeUserItems", "Wipe user items", new SmartConsole.ConsoleCommandFunction(this.WipeUserItems));
            SmartConsole.RegisterCommand("addBots", "addBots RED 2", "Add bots to battle", new SmartConsole.ConsoleCommandFunction(this.AddBotsToBattle));
        }

        public class AdminUserNode : ElevatedAccessUserBaseSystem.SelfUserNode
        {
            public UserAdminComponent userAdmin;
        }
    }
}

