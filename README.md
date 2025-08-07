# Obras-Tolkien-Backend

## Sobre o Projeto

Este backend foi desenvolvido para gerenciar obras relacionadas ao universo de Tolkien, permitindo cadastro de livros, comentários, autenticação de usuários e integração com a OpenLibrary para busca de informações externas.

Principais funcionalidades:
- Cadastro e autenticação de usuários
- Gerenciamento de livros e comentários
- Busca de livros por autor via OpenLibrary
- Documentação via Swagger
- Testes unitários para as principais camadas do sistema

## Estrutura do Projeto

O projeto está organizado em múltiplas soluções e camadas, seguindo boas práticas de arquitetura:

- **TerraMedia.Api**: API principal, responsável pelas rotas e integração das funcionalidades.
- **TerraMedia.Domain**: Entidades, contratos e regras de domínio.
- **TerraMedia.Application**: Camada de aplicação, orquestrando regras de negócio.
- **TerraMedia.Infrastructure**: Persistência de dados e integração com banco de dados.
- **TerraMedia.Integration**: Integração com serviços externos, como a OpenLibrary.

### Testes

O projeto possui testes unitários implementados para as principais camadas:
- `TerraMedia.Domain.Tests`
- `TerraMedia.Infrastructure.Tests`
- `TerraMedia.Integration.Tests`
- `TerraMedia.Api.Tests`
- `TerraMedia.Application.Tests`

Para rodar os testes, utilize o comando:
```sh
dotnet test
```

## Como rodar o projeto com Docker

1. Certifique-se de ter o Docker instalado em sua máquina.
2. Na raiz do projeto, execute o comando abaixo para construir e iniciar o backend:

```sh
docker compose up --build
```

3. O serviço estará disponível na porta `7077` por padrão.

## Dificuldades encontradas

Durante o desenvolvimento, uma das principais dificuldades foi a limitação da API de busca externa (OpenLibrary), que não oferece funcionalidades avançadas de ordenação dos resultados. Isso exigiu adaptações na lógica de apresentação dos dados para garantir uma melhor experiência ao usuário.

## Execute todos os testes com cobertura:

dotnet test TerraMedia.Api.sln --collect:"XPlat Code Coverage"

## Gere o relatório de cobertura em HTML:

reportgenerator "-reports:**/TestResults/*/coverage.cobertura.xml" "-targetdir:BuildReports/Report" -reporttypes:Html

## Pontos pendentes

Ficou faltando a finalização das funcionalidades de cadastro, alteração e exclusão de usuários.

## Documentação

A documentação da API pode ser acessada via Swagger em `/swagger` após iniciar o projeto.

---

**Observação:** O projeto segue boas práticas de separação de responsabilidades, facilitando manutenção,