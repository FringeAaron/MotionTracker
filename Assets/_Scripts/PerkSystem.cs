using System;
using System.Collections.Generic;

public class PerkSystem {
    public event EventHandler OnPerkAdded;    

    private readonly List<Perk> _activePerks;    

    public PerkSystem() {
        _activePerks = new List<Perk>();
    }

    public void AddPerk(Perk perk) {
        _activePerks.Add(perk);            
        OnPerkAdded?.Invoke(this, EventArgs.Empty);
    }

    public List<Perk> GetActivePerks() {
        return _activePerks;
    }
}