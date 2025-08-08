# 🏰 Obras-Tolkien-Backend

## 📖 Sobre o Projeto

Backend desenvolvido para gerenciar obras do universo de Tolkien, com funcionalidades para cadastro de livros, comentários, autenticação de usuários e integração com a OpenLibrary para busca de informações externas.

### Principais funcionalidades:
- Cadastro e autenticação de usuários
- Gerenciamento de livros e comentários
- Busca de livros por autor via OpenLibrary
- Documentação via Swagger
- Testes unitários para as principais camadas do sistema

---

## 🗂️ Estrutura do Projeto

Organizado em múltiplas soluções e camadas, seguindo boas práticas de arquitetura:

| Camada                   | Descrição                                           |
|--------------------------|-----------------------------------------------------|
| **TerraMedia.Api**       | API principal: rotas e integração das funcionalidades |
| **TerraMedia.Domain**    | Entidades, contratos e regras de negócio             |
| **TerraMedia.Application** | Camada de aplicação: orquestra regras de negócio     |
| **TerraMedia.Infrastructure** | Persistência de dados e integração com banco de dados |
| **TerraMedia.Integration** | Integração com serviços externos (ex: OpenLibrary)  |

---

## 🧪 Testes Unitários

Testes implementados para as principais camadas:

- `TerraMedia.Domain.Tests`
- `TerraMedia.Infrastructure.Tests`
- `TerraMedia.Integration.Tests`
- `TerraMedia.Api.Tests`
- `TerraMedia.Application.Tests`

Para executar todos os testes, rode:

```bash
dotnet test
```

---

## 🐳 Como rodar com Docker

1. Certifique-se de ter o Docker instalado.

2. Na raiz do projeto, execute:

```bash
docker compose up --build
```

3. O backend estará disponível na porta padrão `7077`.

---

## ⚠️ Dificuldades Encontradas

A API externa (OpenLibrary) possui limitações, principalmente na ordenação dos resultados, o que exigiu ajustes na lógica de apresentação para melhorar a experiência do usuário.
Informações não padronizadas

---

## 📊 Cobertura de Testes

Para executar os testes com coleta de cobertura:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

```bash
dotnet test TerraMedia.Api.sln --collect:"XPlat Code Coverage"
```

Para gerar o relatório HTML da cobertura:

```bash
reportgenerator "-reports:**/TestResults/*/coverage.cobertura.xml" "-targetdir:BuildReports/Report" -reporttypes:Html
```

---

## 🚧 Pontos Pendentes

- Finalização das funcionalidades de cadastro, alteração e exclusão de usuários.

---

## 📚 Documentação da API

Após iniciar o projeto, acesse a documentação Swagger em:

```
/swagger
```

---

> **Observação:**  
> O projeto segue boas práticas de separação de responsabilidades, facilitando a manutenção e escalabilidade.
