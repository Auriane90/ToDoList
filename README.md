# ToDo List Application

Aplicação de gerenciamento de tarefas com:

* **Backend:** ASP.NET Core (.NET 9)
* **Frontend:** SAPUI5
* **Banco:** SQLite
* **Testes:** xUnit (testes de integração)

---

# Estrutura do Projeto

```
ToDoList
│
├── ToDoListApi
│   ├── Controllers
│   ├── Data
│   ├── Models
│   ├── DTOs
│   ├── wwwroot            # Frontend SAPUI5
│   └── ToDoListApi.csproj
│
└── TodoApi.Tests
```

---

# Pré-requisitos

Antes de rodar o projeto instale:

* [.NET SDK 9](https://dotnet.microsoft.com/)
* Node.js (para UI5)
* UI5 CLI

Instalar UI5 CLI:

```
npm install -g @ui5/cli
```

---

# Executar o Backend

Entrar na pasta da API:

```
cd ToDoListApi
```

Restaurar dependências:

```
dotnet restore
```

Aplicar migrations do banco:

```
dotnet ef database update
```

Rodar a API:

```
dotnet run
```

A API ficará disponível em:

```
http://localhost:5198
```

Swagger:

```
http://localhost:5198/swagger
```

---

# Popular Dados

Para importar tarefas da API pública:

```
POST http://localhost:5198/sync
```

Isso irá importar tarefas de:

```
https://jsonplaceholder.typicode.com/todos
```

---

# Executar o Frontend (SAPUI5)

Abrir no navegador:

```
http://localhost:5198/index.html
```

---

# Executar Testes
link: https://github.com/Auriane90/ToDoListTest.git
Entrar na raiz do projeto:

```
dotnet test
```

Os testes validam:

* filtros
* paginação
* regra de negócio (máximo de 5 tarefas incompletas)
* comportamento dos endpoints GET e PUT

---

# Funcionalidades

* Listagem de tarefas com paginação
* Filtro por título
* Ordenação
* Atualização de status da tarefa
* Regra de negócio: máximo de **5 tarefas incompletas por usuário**
* Sincronização com API externa
* Testes de integração
