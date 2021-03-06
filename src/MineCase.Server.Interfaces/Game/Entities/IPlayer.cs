﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MineCase.Server.Game.Windows;
using MineCase.Server.Network;
using MineCase.Server.User;

namespace MineCase.Server.Game.Entities
{
    public interface IPlayer : IEntity
    {
        Task<string> GetName();

        Task SetName(string name);

        Task BindToUser(IUser user);

        Task<PlayerDescription> GetDescription();

        Task<IInventoryWindow> GetInventory();

        Task SendWholeInventory();

        Task SendExperience();

        Task SendPlayerListAddPlayer(IReadOnlyList<IPlayer> player);

        Task NotifyLoggedIn();

        Task SendPositionAndLook();

        Task OnTeleportConfirm(uint teleportId);

        Task SetPositionAndLook(double x, double feetY, double z, float yaw, float pitch, bool onGround);
    }
}
