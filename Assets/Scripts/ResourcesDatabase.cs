using System.Collections.Generic;
using System.Linq;

public class ResourcesDatabase
{
    private readonly HashSet<Resource> _resources = new();

    public void ReserveCrystal(Resource resource) =>
        _resources.Add(resource);

    public IEnumerable<Resource> GetFreeCrystals(IEnumerable<Resource> resources) =>
        resources.Where(resource => _resources.Contains(resource) == false);

    public void RemoveReservation(Resource resource) =>
        _resources.Remove(resource);
}