import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { IntermediateCode } from './components/IntermediateCode';
import { Automaton } from './components/Automaton';
import { Grammar } from './components/Grammar';
import { RegexEngine } from './components/RegexEngine';
import { Tokenizer } from './components/Tokenizer';
import { AST } from './components/AST';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/regex' component={RegexEngine} />
        <Route path='/tokenizer' component={Tokenizer} />
        <Route path='/grammar' component={Grammar} />
        <Route path='/automaton' component={Automaton} />
        <Route path='/il' component={IntermediateCode} />
        <Route path='/ast' component={AST} />
      </Layout>
    );
  }
}
