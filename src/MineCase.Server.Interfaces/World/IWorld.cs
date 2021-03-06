﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MineCase.Server.Game;
using Orleans;

namespace MineCase.Server.World
{
    public interface IWorld : IGrainWithStringKey
    {
        Task<uint> NewEntityId();

        Task AttachEntity(IEntity entity);

        Task<IEntity> FindEntity(uint eid);

        Task<(long age, long timeOfDay)> GetTime();
    }
}
