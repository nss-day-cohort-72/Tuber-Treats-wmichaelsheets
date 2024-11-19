using TuberTreats.Models;
using TuberTreats.Models.DTOs;

List<TuberDriver> tuberDrivers = new List<TuberDriver>
{
    new TuberDriver
    {
        Id = 1,
        Name = "John Doe",
        TuberDeliveries = new List<TuberDelivery>()
    },
    new TuberDriver
    {
        Id = 2,
        Name = "Jane Smith",
        TuberDeliveries = new List<TuberDelivery>()
    },
    new TuberDriver
    {
        Id = 3,
        Name = "Mike Johnson",
        TuberDeliveries = new List<TuberDelivery>()
    }
};

List<Customer> customers = new List<Customer>
{
    new Customer
    {
        Id = 1,
        Name = "Alice Johnson",
        Address = "123 Oak St, Anytown, USA",
        TuberOrders = new List<TuberOrder>()
    },
    new Customer
    {
        Id = 2,
        Name = "Bob Smith",
        Address = "456 Elm St, Otherville, USA",
        TuberOrders = new List<TuberOrder>()
    },
    new Customer
    {
        Id = 3,
        Name = "Charlie Brown",
        Address = "789 Pine St, Somewhere, USA",
        TuberOrders = new List<TuberOrder>()
    },
    new Customer
    {
        Id = 4,
        Name = "Diana Ross",
        Address = "101 Maple Ave, Elsewhere, USA",
        TuberOrders = new List<TuberOrder>()
    },
    new Customer
    {
        Id = 5,
        Name = "Ethan Hunt",
        Address = "202 Birch Rd, Nowheresville, USA",
        TuberOrders = new List<TuberOrder>()
    }
};

List<Topping> toppings = new List<Topping>
{
    new Topping
    {
        Id = 1,
        Name = "Cheese"
    },
    new Topping
    {
        Id = 2,
        Name = "Bacon"
    },
    new Topping
    {
        Id = 3,
        Name = "Sour Cream"
    },
    new Topping
    {
        Id = 4,
        Name = "Chives"
    },
    new Topping
    {
        Id = 5,
        Name = "Butter"
    }
};

List<TuberOrder> tuberOrders = new List<TuberOrder>
{
    new TuberOrder
    {
        Id = 1,
        OrderPlacedOnDate = DateTime.Now.AddHours(-2),
        CustomerId = 1,
        TuberDriverId = 1,
        DeliveredOnDate = DateTime.Now.AddHours(-1),
        Toppings = new List<Topping>
        {
            toppings[0], // Cheese
            toppings[1]  // Bacon
        }
    },
    new TuberOrder
    {
        Id = 2,
        OrderPlacedOnDate = DateTime.Now.AddHours(-3),
        CustomerId = 2,
        TuberDriverId = 2,
        DeliveredOnDate = DateTime.Now.AddHours(-2),
        Toppings = new List<Topping>
        {
            toppings[2], // Sour Cream
            toppings[3], // Chives
            toppings[4]  // Butter
        }
    },
    new TuberOrder
    {
        Id = 3,
        OrderPlacedOnDate = DateTime.Now.AddHours(-1),
        CustomerId = 3,
        TuberDriverId = 3,
        DeliveredOnDate = DateTime.Now,
        Toppings = new List<Topping>()
    }
};



var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//add endpoints here
// ------------------------------------------ tuberorders endpoints ------------------------------------------
app.MapGet("/tuberorders", () =>
{
    return tuberOrders.Select(t => new TuberOrderDTO
    {
        Id = t.Id,
        OrderPlacedOnDate = t.OrderPlacedOnDate,
        CustomerId = t.CustomerId,
        TuberDriverId = t.TuberDriverId,
        DeliveredOnDate = t.DeliveredOnDate,
        ToppingNames = t.Toppings.Select(topping => topping.Name).ToList()
    }).ToList();
});

app.MapGet("/tuberorders/{id}", (int id) =>
{
    var order = tuberOrders.FirstOrDefault(t => t.Id == id);

    var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);

    var driver = tuberDrivers.FirstOrDefault(d => d.Id == order.TuberDriverId);

    var orderDto = new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlacedOnDate = order.OrderPlacedOnDate,
        CustomerId = order.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            TuberOrders = new List<TuberOrder>()
        },
        TuberDriverId = order.TuberDriverId,
        Driver = new TuberDriverDTO
        {
            Id = driver.Id,
            Name = driver.Name,
            TuberDeliveries = new List<TuberDelivery>()
        },
        DeliveredOnDate = order.DeliveredOnDate,
        ToppingNames = order.Toppings.Select(topping => topping.Name).ToList(),
    };

    return Results.Ok(orderDto);
});

app.MapPost("/tuberorders", (TuberOrder tuberOrder) =>
{
    Customer customer = customers.FirstOrDefault(c => c.Id == tuberOrder.CustomerId);

    tuberOrder.Id = tuberOrders.Max(o => o.Id) + 1;

    tuberOrder.OrderPlacedOnDate = DateTime.Now;

    tuberOrders.Add(tuberOrder);

    customer.TuberOrders.Add(tuberOrder);

    return Results.Created($"/tuberorders/{tuberOrder.Id}", new TuberOrderDTO
    {
        Id = tuberOrder.Id,
        OrderPlacedOnDate = tuberOrder.OrderPlacedOnDate,
        CustomerId = tuberOrder.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            TuberOrders = new List<TuberOrder>()
        },
        TuberDriverId = tuberOrder.TuberDriverId,
        Driver = new TuberDriverDTO
        {
            Id = tuberDrivers.First(d => d.Id == tuberOrder.TuberDriverId).Id,
            Name = tuberDrivers.First(d => d.Id == tuberOrder.TuberDriverId).Name,
            TuberDeliveries = new List<TuberDelivery>()
        },
        Toppings = tuberOrder.Toppings.Select(t => new ToppingDTO
        {
            Id = t.Id,
            Name = t.Name
        }).ToList(),
    }

    );
});
//The above works with this object:
// {
//   "customerId": 1,
//   "tuberDriverId": 2,
//   "toppings": [
//     {
//       "id": 1,
//       "name": "Cheese"
//     },
//     {
//       "id": 2,
//       "name": "Bacon"
//     }
//   ]
// }

app.MapPut("/tuberorders/{id}", (int id, TuberOrder updatedOrder) =>
{
    var existingOrder = tuberOrders.FirstOrDefault(o => o.Id == id);

    var driver = tuberDrivers.FirstOrDefault(d => d.Id == updatedOrder.TuberDriverId);

    existingOrder.TuberDriverId = updatedOrder.TuberDriverId;

    var customer = customers.FirstOrDefault(c => c.Id == existingOrder.CustomerId);

    var updatedOrderDto = new TuberOrderDTO
    {
        Id = existingOrder.Id,
        OrderPlacedOnDate = existingOrder.OrderPlacedOnDate,
        CustomerId = existingOrder.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            TuberOrders = new List<TuberOrder>()
        },
        TuberDriverId = existingOrder.TuberDriverId,
        Driver = new TuberDriverDTO
        {
            Id = driver.Id,
            Name = driver.Name,
            TuberDeliveries = new List<TuberDelivery>()
        },
        DeliveredOnDate = existingOrder.DeliveredOnDate,
        ToppingNames = existingOrder.Toppings.Select(topping => topping.Name).ToList()
    };

    return Results.Ok(updatedOrderDto);
});

// the above works with this object:
// {
//   "id": 1,
//   "orderPlacedOnDate": "2023-06-15T10:00:00",
//   "customerId": 1,
//   "tuberDriverId": 2,
//   "deliveredOnDate": "2023-06-15T11:00:00",
//   "toppings": [
//     {
//       "id": 1,
//       "name": "Cheese"
//     },
//     {
//       "id": 2,
//       "name": "Bacon"
//     }
//   ]
// }

app.MapPost("/tuberorders/{id}/complete", (int id) =>
{
    var order = tuberOrders.FirstOrDefault(o => o.Id == id);

    order.Complete = true;

    order.DeliveredOnDate = DateTime.Now;

    var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);

    var driver = tuberDrivers.FirstOrDefault(d => d.Id == order.TuberDriverId);

    var completedOrderDto = new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlacedOnDate = order.OrderPlacedOnDate,
        CustomerId = order.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            TuberOrders = new List<TuberOrder>()
        },
        TuberDriverId = order.TuberDriverId,
        Driver = new TuberDriverDTO
        {
            Id = driver.Id,
            Name = driver.Name,
            TuberDeliveries = new List<TuberDelivery>()
        },
        DeliveredOnDate = order.DeliveredOnDate,
        ToppingNames = order.Toppings.Select(topping => topping.Name).ToList(),
        Complete = order.Complete
    };

    return Results.Ok(completedOrderDto);
});
//------------------------------------------ toppings endpoints ------------------------------------------

app.MapGet("/toppings", () =>
{
    return toppings.Select(t => new ToppingDTO
    {
        Id = t.Id,
        Name = t.Name
    }).ToList();
});

app.MapGet("/toppings/{id}", (int id) =>
{
    var top = toppings.FirstOrDefault(t => t.Id == id);

    var topping = new ToppingDTO
    {
        Id = top.Id,
        Name = top.Name

    };

    return Results.Ok(topping);
});

//------------------------------------------ tubertoppings endpoints ------------------------------------------

app.MapGet("/tubertoppings", () =>
{
    var tuberToppings = tuberOrders.Select(order => new
    {
        TuberOrderId = order.Id,
        Toppings = order.Toppings.Select(topping => new
        {
            ToppingId = topping.Id,
            ToppingName = topping.Name
        }).ToList()
    }).ToList();

    return Results.Ok(tuberToppings);
});

app.MapPost("/tubertoppings", (TuberTopping tuberTopping) =>
{
    var order = tuberOrders.FirstOrDefault(o => o.Id == tuberTopping.TuberOrderId);

    var topping = toppings.FirstOrDefault(t => t.Id == tuberTopping.ToppingId);

    order.Toppings.Add(topping);

    tuberTopping.Id = order.Toppings.Count;

    var newTuberToppingDTO = new TuberToppingDTO
    {
        Id = tuberTopping.Id,
        TuberOrderId = tuberTopping.TuberOrderId,
        ToppingId = tuberTopping.ToppingId,
        ToppingName = topping.Name
    };

    return Results.Created($"/tubertoppings/{newTuberToppingDTO.Id}", newTuberToppingDTO);
});

app.MapDelete("/tubertoppings/{orderId}/{toppingId}", (int orderId, int toppingId) =>
{
    var order = tuberOrders.FirstOrDefault(o => o.Id == orderId);

    var topping = order.Toppings.FirstOrDefault(t => t.Id == toppingId);

    order.Toppings.Remove(topping);

    return Results.Ok(new
    {
        OrderId = orderId,
        RemovedToppingId = toppingId,
        RemainingToppings = order.Toppings.Select(t => new { t.Id, t.Name }).ToList()
    });
});

//------------------------------------------ customers endpoints ------------------------------------------

app.MapGet("/customers", () =>
{
    return customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address,
        TuberOrders = tuberOrders
            .Where(o => o.CustomerId == c.Id)
            .Select(o => new TuberOrder
            {
                Id = o.Id,
                OrderPlacedOnDate = o.OrderPlacedOnDate,
                CustomerId = o.CustomerId,
                TuberDriverId = o.TuberDriverId,
                DeliveredOnDate = o.DeliveredOnDate,
                Toppings = o.Toppings.ToList()
            })
            .ToList()
    }).ToList();
});

app.MapGet("/customers/{id}", (int id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);

    var customerDTO = new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address,
        TuberOrders = tuberOrders
            .Where(o => o.CustomerId == customer.Id)
            .Select(o => new TuberOrder
            {
                Id = o.Id,
                OrderPlacedOnDate = o.OrderPlacedOnDate,
                CustomerId = o.CustomerId,
                TuberDriverId = o.TuberDriverId,
                DeliveredOnDate = o.DeliveredOnDate,
                Toppings = o.Toppings.ToList()
            })
            .ToList()
    };

    return Results.Ok(customerDTO);
});

app.MapPost("/customers", (Customer newCustomer) =>
{
    newCustomer.Id = customers.Max(c => c.Id) + 1;

    newCustomer.TuberOrders = new List<TuberOrder>();

    customers.Add(newCustomer);

    var customerDTO = new CustomerDTO
    {
        Id = newCustomer.Id,
        Name = newCustomer.Name,
        Address = newCustomer.Address,
        TuberOrders = new List<TuberOrder>()
    };

    return Results.Created($"/customers/{newCustomer.Id}", customerDTO);
});

//the above works with this object:
// {
//   "name": "New Customer",
//   "address": "123 New Street, Newtown, USA"
// }

app.MapDelete("/customers/{id}", (int id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);

    customers.Remove(customer);
    return Results.Ok($"Customer with ID {id} has been deleted.");
});

//------------------------------------------ tuberdrivers endpoints ------------------------------------------

app.MapGet("/tuberdrivers", () =>
{
    return tuberDrivers.Select(d => new
    {
        Id = d.Id,
        Name = d.Name
    }).ToList();
});

app.MapGet("/tuberdrivers/{id}", (int id) =>
{
    var driver = tuberDrivers.FirstOrDefault(d => d.Id == id);

    var driverWithDeliveries = new
    {
        Id = driver.Id,
        Name = driver.Name,
        TuberDeliveries = tuberOrders
            .Where(o => o.TuberDriverId == driver.Id)
            .Select(o => new
            {
                Id = o.Id,
                OrderPlacedOnDate = o.OrderPlacedOnDate,
                CustomerId = o.CustomerId,
                CustomerName = customers.FirstOrDefault(c => c.Id == o.CustomerId)?.Name,
                DeliveredOnDate = o.DeliveredOnDate,
                Toppings = o.Toppings.Select(t => t.Name).ToList()
            })
            .ToList()
    };

    return Results.Ok(driverWithDeliveries);
});


app.Run();



//don't touch or move this!
public partial class Program { }