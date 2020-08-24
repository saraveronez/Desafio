import { Component } from '@angular/core';
import { HomeService } from './home.service';

import { Divida } from '../models/Divida';

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent {

  public dividas: Divida[];
  constructor(private dividaService: HomeService) { }

  ngOnInit(): void {
    this.carregarDividas();
  }

  carregarDividas() {
    this.dividaService.getDividas().subscribe(
      (dividas: Divida[]) => {
        this.dividas = dividas;
      },
      (erro: any) => {
        console.log(erro);
      }
    );
  }
}

