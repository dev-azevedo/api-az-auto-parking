# 🚘 Az Auto Parking 🚘 By: Jhonatan Azevedo

## 🗃️ Database
![Diagrama](docs/drawSQL-az-auto-parking.png)

[Database diagram link](https://drawsql.app/teams/jhonatan-azevedo/diagrams/az-auto-parking-by-dev-azevedo)

## 🎡 How to run migrations?
Run command in Infra layer
```bash
  dotnet ef database update --context AppDbContext
```

## 🧪 How to test?
documentation with postman [here](docs/AzAutoParking.postman_collection.json).
Import in Postman

### Api for get data automobile
var type = 'carros' | 'motos' | 'caminhoes'
https://parallelum.com.br/fipe/api/v1/{type}/marcas
Return: 
```bash
{
    "codigo": "14",
    "nome": "Cross Lander"
}
```
https://parallelum.com.br/fipe/api/v1/{type}/marcas/{codigo}/modelos
Return: 
```bash
{
    "codigo": 437,
    "nome": "147 C/ CL"
}
```

