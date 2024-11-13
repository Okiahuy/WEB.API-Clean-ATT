﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Cart
{
    public interface ICartService
    {
        Task AddToCartAsync(int accountID, int productID, int quantity);
    }
}
