# Normalizador de Dados para Investigação

Mini projeto de validação e normalização de registros de ligações telefônicas em CSV, com exportação estruturada para importação no i2 Analyst’s Notebook.

## Objetivo

Preparar registros de ligações telefônicas para apoiar análises de comunicação entre números de telefone.

O projeto atende ao cenário em que um analista recebe um arquivo CSV e precisa organizar os dados para visualizar:

- Quem se comunica com quem.
- Quando as ligações ocorreram.
- Com que frequência os telefones se comunicam.
- Qual a duração das ligações.

## Escopo

Cada linha do arquivo de entrada representa uma tentativa de ligação entre um telefone de origem e um telefone de destino.

O processamento deve validar os campos obrigatórios, rejeitar registros inválidos sem interromper a execução e exportar os registros aceitos em um CSV estruturado.

Além da exportação, o sistema deve gerar um relatório com os resultados do processamento e os motivos das rejeições.

## Requisitos funcionais

- Receber o caminho do CSV de entrada.
- Receber o caminho do CSV de exportação.
- Validar telefone de origem, telefone de destino, data/hora, duração e tipo da ligação.
- Rejeitar registros inválidos sem interromper o processamento das demais linhas.
- Gerar um relatório com:
  - Total de registros lidos.
  - Total de registros aceitos.
  - Total de registros rejeitados.
  - Motivo de cada rejeição.
- Exportar somente as ligações válidas.
- Representar cada ligação por meio de duas entidades do tipo telefone e um link do tipo ligação.

## Fluxo de processamento

```text
Informação dos caminhos de entrada e saída
                    |
                    v
           Leitura do CSV de entrada
                    |
                    v
           Validação de cada registro
                    |
          +---------+---------+
          |                   |
          v                   v
   Registro válido     Registro inválido
          |                   |
          v                   v
 Preparação para       Registro do motivo
    exportação            da rejeição
          |                   |
          +---------+---------+
                    |
                    v
     Exportação das ligações válidas
                    |
                    v
     Geração do relatório de processamento
```

## Dados de entrada

O usuário deve informar o caminho de um arquivo CSV em que cada linha represente uma tentativa de ligação.

### Campos obrigatórios

| Campo | Descrição |
| --- | --- |
| Origem | Telefone que iniciou a tentativa de ligação. |
| Destino | Telefone para o qual a ligação foi direcionada. |
| Data/hora | Data e horário da tentativa de ligação. |
| Duração | Duração registrada para a ligação. |
| Tipo | Classificação da ligação. |

Os nomes físicos das colunas, os formatos de data/hora, a unidade de duração e os valores permitidos para o tipo devem seguir o contrato de dados adotado na implementação.

## Validação dos registros

Cada registro deve ser validado antes de ser incluído no arquivo de exportação.

| Campo | Validação requerida |
| --- | --- |
| Origem | Verificar se o telefone está preenchido e atende ao formato aceito pelo sistema. |
| Destino | Verificar se o telefone está preenchido e atende ao formato aceito pelo sistema. |
| Data/hora | Verificar se o valor representa uma data e um horário válidos no formato aceito. |
| Duração | Verificar se o valor atende ao formato, à unidade e aos limites definidos pelo sistema. |
| Tipo | Verificar se o valor corresponde a uma classificação aceita. |

### Tratamento de registros inválidos

Quando um registro apresentar dados inválidos:

1. O registro deve ser rejeitado.
2. O motivo da rejeição deve ser registrado no relatório.
3. O processamento deve continuar para os demais registros.
4. O registro rejeitado não deve ser incluído no CSV de exportação.

As regras específicas de normalização e os valores aceitos devem ser definidos na implementação. Os requisitos não estabelecem, por exemplo, códigos de país obrigatórios, duração mínima ou uma lista de tipos de ligação.

## Modelo de dados para análise

Cada ligação válida deve ser representada pelo seguinte relacionamento:

```text
[Telefone de origem] ─── Ligação ───> [Telefone de destino]
```

### Entidades

| Elemento | Tipo | Identificação |
| --- | --- | --- |
| Entidade de origem | Telefone | Número de origem. |
| Entidade de destino | Telefone | Número de destino. |

### Link

| Elemento | Tipo | Descrição |
| --- | --- | --- |
| Relacionamento | Ligação | Tentativa de comunicação entre origem e destino. |

### Propriedades da ligação

| Propriedade | Descrição |
| --- | --- |
| Data/hora | Momento em que ocorreu a tentativa de ligação. |
| Duração | Duração da ligação, conforme a unidade adotada pelo projeto. |
| Tipo | Classificação da ligação. |
| Direção | Sentido da ligação, da origem para o destino. |

O telefone de origem e o telefone de destino devem permanecer distintos no mapeamento, preservando o sentido da comunicação.

## CSV de exportação

O arquivo de saída deve conter somente registros válidos e informações suficientes para mapear:

- A entidade de origem como telefone.
- A entidade de destino como telefone.
- O relacionamento como ligação.
- As propriedades de data/hora, duração, tipo e direção.

### Estrutura lógica

| Informação | Destino no modelo de análise |
| --- | --- |
| Telefone de origem | Identificador da entidade de origem. |
| Telefone de destino | Identificador da entidade de destino. |
| Ligação | Tipo do link entre as entidades. |
| Data/hora | Propriedade do link. |
| Duração | Propriedade do link. |
| Tipo | Propriedade do link. |
| Direção | Orientação do link entre origem e destino. |

Essa estrutura descreve o conteúdo necessário à exportação. Os nomes definitivos das colunas e o respectivo mapeamento de importação devem acompanhar a implementação.

## Relatório de processamento

Ao final da execução, o sistema deve apresentar um resumo com os seguintes indicadores:

| Indicador | Descrição |
| --- | --- |
| Total lido | Quantidade de registros processados do CSV de entrada. |
| Total aceito | Quantidade de registros aprovados na validação. |
| Total rejeitado | Quantidade de registros não aprovados. |

Os totais devem respeitar a relação:

```text
Total lido = Total aceito + Total rejeitado
```

O relatório também deve informar o motivo de cada rejeição, permitindo identificar os problemas que precisam ser corrigidos na origem dos dados.

### Exemplo ilustrativo

```text
RESUMO DO PROCESSAMENTO

Total lido:       100
Total aceito:      92
Total rejeitado:    8

EXEMPLOS DE REJEIÇÃO

Registro 12: telefone de origem ausente.
Registro 27: data/hora inválida.
Registro 43: duração em formato inválido.
Registro 68: tipo de ligação não aceito.
```

Os números e as mensagens acima são apenas ilustrativos.

## Como utilizar

O fluxo de uso previsto é:

1. Disponibilizar o CSV com os registros de ligações.
2. Informar o caminho do arquivo de entrada.
3. Informar o caminho desejado para o CSV de exportação.
4. Executar o processamento.
5. Consultar o relatório de aceitação e rejeição.
6. Utilizar o CSV exportado no fluxo de importação do i2 Analyst’s Notebook, mapeando as entidades, o link e suas propriedades.

Os comandos de instalação e execução devem ser documentados conforme a tecnologia utilizada no projeto.

## Critérios de aceitação

- [ ] O usuário consegue informar os caminhos de entrada e exportação.
- [ ] Cada linha de dados é tratada como uma tentativa de ligação.
- [ ] Origem, destino, data/hora, duração e tipo são validados.
- [ ] Registros inválidos são rejeitados individualmente.
- [ ] Uma rejeição não interrompe o processamento das demais linhas.
- [ ] O relatório informa os totais de registros lidos, aceitos e rejeitados.
- [ ] Cada registro rejeitado possui um motivo registrado.
- [ ] O CSV de exportação contém somente ligações válidas.
- [ ] Cada ligação exportada permite identificar as entidades de origem e destino.
- [ ] Cada ligação exportada contém data/hora, duração, tipo e direção.

## Limites do escopo

Os requisitos apresentados não definem:

- Linguagem de programação ou bibliotecas.
- Interface de uso: terminal, aplicação web ou interface gráfica.
- Nomes exatos das colunas.
- Codificação e delimitador dos arquivos CSV.
- Formato e fuso horário dos campos de data/hora.
- Unidade de medida da duração.
- Tipos de ligação permitidos.
- Regras para números nacionais e internacionais.
- Tratamento de duplicidades.
- Formato e local de armazenamento do relatório.
- Configuração específica do mapeamento de importação no i2 Analyst’s Notebook.

Essas decisões devem ser registradas conforme forem implementadas.

## Cuidados com os dados

Os arquivos utilizados no projeto podem conter informações sensíveis de comunicação.

Para exemplos, demonstrações e testes publicados no repositório, utilize dados fictícios. Não inclua registros reais de ligações, números pessoais ou outros dados confidenciais.

## Autor

[Joavl](https://github.com/Joavl)
