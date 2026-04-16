# Transaction

## Descrição

Representa um registro financeiro pertencente a um usuário, podendo ser uma receita ou despesa.

---

## Campos

- **Id**: Identificador único da transação
- **Description**: Descrição textual da transação
- **Amount**: Valor financeiro (positivo ou negativo)
- **Date**: Data da transação
- **CategoryId**: Identificador da categoria associada
- **UserId**: Identificador do usuário proprietário

---

## Regras de Negócio (Invariantes)

1. **Description**
   - Obrigatória
   - Não pode ser vazia ou apenas espaços
   - Deve ter no máximo 200 caracteres

2. **Amount**
   - Não pode ser zero
   - Pode ser:
     - Positivo → receita
     - Negativo → despesa

3. **Date**
   - Não pode ser no futuro

4. **CategoryId**
   - Deve ser válido (≠ Guid.Empty)
   - Deve existir no sistema *(validado fora da entidade)*

5. **UserId**
   - Deve ser válido (≠ Guid.Empty)

---

## Observações

- A entidade garante apenas invariantes internas.
- A existência da categoria deve ser validada na camada de aplicação.