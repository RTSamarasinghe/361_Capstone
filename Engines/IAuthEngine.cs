using System;
using System.Collections.Generic;
using System.Text;

namespace Engines;

    public interface IAuthEngine
    {
        string GenerateJwtToken(int customerId, string key);
    }

