import React, { Component } from 'react';

export class Automaton extends Component {
    static displayName = Automaton.name;

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    componentDidMount() {
        this.getAutomaton();
    }

    getAutomaton() {
      fetch(process.env.apiUrl + "/api/compiler/automaton")
        .then(res => res.json())
          .then(res => {
             let viz = new window.Viz({ workerURL: 'full.render.js'} );

             viz.renderSVGElement(res.automaton)
            .then(element => {
                this.refs.graphContainer.append(element);
                this.setState({ loading: false });
            })
            .catch(error => {
                viz = new window.Viz({ workerURL: 'full.render.js'} );
                console.error(error);
            });

        });
    }

    render() {
        return (
            <div className="automaton">
                <h1>Automaton</h1>

                {
                  this.state.loading && <p>Loading....</p>
                }

                {
                  !this.state.loading && <p>Scroll to the right</p>
                }

                <div ref="graphContainer" className="graphContainer">
                </div>
            </div>
        );
    }
}
