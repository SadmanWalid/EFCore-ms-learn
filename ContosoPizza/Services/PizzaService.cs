using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly PizzaContext _dbContext;
    public PizzaService(PizzaContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Pizza> GetAll()
    {
        return _dbContext.Pizzas.
        AsNoTracking().ToList();
    }

    public Pizza? GetById(int id)
    {
        return _dbContext.Pizzas
        .Include(t => t.Toppings)
        .Include(s => s.Sauce)
        .AsNoTracking()
        .SingleOrDefault(p => p.Id == id);
    }

    public Pizza? Create(Pizza newPizza)
    {
        _dbContext.Pizzas.Add(newPizza);
        _dbContext.SaveChanges();
        return newPizza;
    }

    public void AddTopping(int PizzaId, int ToppingId)
    {
        var pizzaToUpdate = _dbContext.Pizzas.Find(PizzaId);
        var toppingToAdd = _dbContext.Toppings.Find(ToppingId);

        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new InvalidOperationException("Pizza or topping does not exit");
        }

        if (pizzaToUpdate.Toppings is null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }
        pizzaToUpdate.Toppings.Add(toppingToAdd);

        _dbContext.SaveChanges();

    }

    public void UpdateSauce(int PizzaId, int SauceId)
    {
        var pizzaToUpdate = _dbContext.Pizzas.Find(PizzaId);
        var sauceToAdd = _dbContext.Sauces.Find(SauceId);

        if (pizzaToUpdate is null || sauceToAdd is null)
        {
            throw new InvalidOperationException("Pizza or sauce does not exist");
        }

        pizzaToUpdate.Sauce = sauceToAdd;

        _dbContext.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizzaToDelete = _dbContext.Pizzas.Find(id);

        if (pizzaToDelete is not null)
        {
            _dbContext.Pizzas.Remove(pizzaToDelete);
            _dbContext.SaveChanges();
        }
    }
}