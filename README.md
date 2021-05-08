# Projeto de conclusão de curso - Universidade Mogi das Cruzes - Sistemas de Informação

## Conclusão do curso: 2020 ✅

# Tecnologias Usadas:
## Back-end:
<ul>
  <li>.Net Core 3.1</li>  
  <li>Entity Framework Core</li>
  <li>PostgreSql</li>
  <li>Swagger</li>
</ul>

## Front-end:
<ul>
  <li>Angular 10.0</li>
  <li>Firebase</li>
  <li>Bootstrap</li>
  <li>Typescript</li>
</ul>


# Pré requisitos
💻 SDK .NET Core 3.1.2+
💻 EF Core 3.1.2+
💻 Ambiente para PostgreSql

# Como utilizar

<ul>
  <li>Abra o projeto do <strong>back-end</strong></li>
  <li>Configure as credenciais de acordo com seu banco de dados PostgreSql no arquivo <i>TICKET2U-API/Data/DataContext.cs</i></li>
  <li>Dentro do diretório TICKET2U-API execute <code>dotnet ef database update</code> para mapear os objetos das migrations para seu banco de dados</li>
  <li>Após isso, execute <code>dotnet run</code> para executar a aplicação na porta 5000 e 5001</li>
  <li>Abra o projeto do <strong>front-end</strong></li>
  <li>Dentro do diretório TICKET2U-APP execute <code>npm i</code> para instalar os pacotes</li>
  <li>Após a instalação, execute a aplicação utilizando <code>ng serve -o</code> e irá abrir a janela da aplicação no seu navegador padrão</li>
</ul>