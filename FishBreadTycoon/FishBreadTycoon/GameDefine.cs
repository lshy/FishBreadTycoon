using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishBreadTycoon
{
    enum TEEL_STATE
    {
        OJ_IDLE,
        OJ_BASEING,
        OJ_BASE,
        OJ_PATING,
        OJ_PAT,
        OJ_REVERSEING,
        OJ_END,
        OJ_BURNING,
    }
    enum RETURN_FLAG
    {
        NULL,
        DELETE,
    }
    enum CURSOR_TYPE
    {
        HAND,
        KETTLE,
        PAT,
    }
    enum SPRITE_COUNT
    {
        SP_BASEING = 10,
        SP_PATING = 3,
        SP_REVERSING = 4,
        SP_BURNING = 5,
    }
}
