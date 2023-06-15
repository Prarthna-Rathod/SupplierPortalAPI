﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SupplierRoot.ValueObjects;

public class SupplierVO
{
    public SupplierVO(int id, string name, bool isActive, IEnumerable<FacilityVO> facilities, IEnumerable<UserVO> users)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        Facilities = facilities;
        Users = users;
    }
    public SupplierVO()
    {

    }

    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public IEnumerable<FacilityVO> Facilities { get; set; }

    public IEnumerable<UserVO> Users { get; set; }
}
