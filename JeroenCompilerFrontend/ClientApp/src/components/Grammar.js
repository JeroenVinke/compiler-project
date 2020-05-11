import React, { Component } from 'react';

export class Grammar extends Component {
    static displayName = Grammar.name;

    constructor(props) {
        super(props);
        this.state = { grammar: [] };
    }

    componentDidMount() {
        this.getGrammar();
    }

    getGrammar() {
      fetch("/api/compiler/grammar")
        .then(res => res.json())
        .then(res => {
            this.setState({ grammar: res.productions });
        });
    }

    render() {
        return (
            <div className="grammar">
                <h1>Grammar</h1>
                <p>Each line contains a production head (written in bold) and a production body (everything after the -&gt;).<br/>
                Terms in the production body written italic are non terminal expressions. These can be replaced by a production body which has the term as the production head.</p>
                {
                    this.state.grammar.map(production => {
                        return production.subProductions.map(subproduction => {
                            return (
                                <p><span style={{fontWeight: 'bold' }}>{production.head}</span> -&gt; {subproduction.expressions.map(expr => {
                                  if (expr.isNonTerminalExpression) {
                                    return <span style={{fontStyle: 'italic' }}>{expr.name}&nbsp;</span>
                                  }
                                  else {
                                    return <span>{expr.name}&nbsp;</span>
                                  }
                                })}</p>
                            );
                        });
                    })
                }
            </div>
        );
    }
}
