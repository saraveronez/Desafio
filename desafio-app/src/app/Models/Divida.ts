import { DividaParcela } from './DividaParcela';

export class Divida {
  id: string;
  valorTotal: number;
  numeroParcelas: number;
  dataVencimento: string;
  quantidadeMaximaParcelas: number;
  valorComJuros: number;
  parcelas: DividaParcela[];
}
