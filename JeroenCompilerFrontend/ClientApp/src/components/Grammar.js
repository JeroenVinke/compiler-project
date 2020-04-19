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
      fetch(process.env.apiUrl + "/api/compiler/grammar")
        .then(res => res.json())
        .then(res => {
            this.setState({ grammar: res.productions });
        });
    }

    render() {
        return (
            <div className="grammar">
                <h1>Grammar</h1>
                {
                    this.state.grammar.map(production => {
                        return production.subProductions.map(subproduction => {
                            return (
                                <p>{production.head} -> {subproduction.expressions.join(' ')}</p>
                            );
                        });
                    })
                }
            </div>
        );
    }
}
