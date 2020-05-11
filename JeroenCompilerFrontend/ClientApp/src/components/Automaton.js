import React, { Component } from 'react';

export class Automaton extends Component {
    static displayName = Automaton.name;

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    componentDidMount() {
    }

    render() {
        return (
            <div className="automaton">
                <h1>Automaton</h1>
                <p>Image may take a while to load</p>
                <div ref="graphContainer" className="graphContainer">
                  <img src="automaton.svg" alt=""/>
                </div>
            </div>
        );
    }
}
