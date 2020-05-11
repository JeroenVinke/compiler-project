import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Compiler</h1>
        <p>This is a demo of my bottom-up LR(1) shift reduce parser with a handwritten regex engine.</p>
        <p>Techniques from the book Compilers: Principles, Techniques, &amp; Tools by Alfred V. Aho et al</p>
        
        <h2>General process</h2>

        <h3>Regex engine</h3>
        <p><span style={{ fontWeight: 'bold' }}>Description:</span> The <a href="/regex">Regular expression engine</a> creates a tree from the regular expression, and translates that tree into a Deterministic Finite Automaton (DFA). This DFA is gone through character for character to see if the input string matches the regular expression.</p>
        <p><span style={{ fontWeight: 'bold' }}>Input:</span> regular expression, input string</p>
        <p><span style={{ fontWeight: 'bold' }}>Output:</span> boolean (match or not)</p>

        <h3>Tokenizer</h3>
        <p><span style={{ fontWeight: 'bold' }}>Description:</span> The <a href="/tokenizer">Tokenizer</a> uses <a href="/regex">regular expressions</a> to identify and mark individual or grouped characters.</p>
        <p>Example: the string "1234" is matched by regex ([0-9])* and is marked as "Integer". The notation for such token is: &lt;Integer, 1234&gt;</p>
        <p><span style={{ fontWeight: 'bold' }}>Input:</span> source text, regular expressions together with the token type</p>
        <p><span style={{ fontWeight: 'bold' }}>Output:</span> a list of Tokens</p>

        <h3>Grammar rules</h3>
        <p><span style={{ fontWeight: 'bold' }}>Description:</span> A <a href="/grammar">grammar rule</a> (production) has two parts: a head and a body. </p>
        <p>Example: NumericExpression -> NumericExpression + NumericExpression | Number</p>
        <p>In this example "Number" is a terminal (0/1/2/3/4/5/6/7/8/9) and "NumericExpression" is a non-terminal. A "NumericExpression" can either be the addition of a "NumericExpression" with another "NumericExpression", or it can be a "Number"</p>
        <p>A non-terminal can be seen as a placeholder for another nonterminal or terminal.</p>
        <p>With this grammar rule, valid input text could be "1" or "1+1" or "1+1+1+1....+1"</p>

        <h3>Parser</h3>
        <p><span style={{ fontWeight: 'bold' }}>Description:</span> The parser receives tokens one by one from the tokenizer. The goal is to match the tokens to the <a href="/grammar">grammar rules</a>. First, the grammar rules are translated into a <a href="/automaton">LR(1) automaton</a></p>
        <p>During parsing, an <a href="/ast">abstract syntax tree</a> is created.</p>
        <p><span style={{ fontWeight: 'bold' }}>Input:</span> a list of tokens from the tokenizer, a list of grammar rules</p>
        <p><span style={{ fontWeight: 'bold' }}>Output:</span> abstract syntax tree</p>

        <h3>Intermediate Language</h3>
        <p><span style={{ fontWeight: 'bold' }}>Description:</span> Based on the <a href="/ast">abstract syntax tree</a>, IL (Intermediate Language) code is generated. The result is <a href="/il">three address code</a>.</p>
        <p><span style={{ fontWeight: 'bold' }}>Input:</span> abstract syntax tree</p>
        <p><span style={{ fontWeight: 'bold' }}>Output:</span> three address code</p>

        <h3>Assembly</h3>
        <p><span style={{ fontWeight: 'bold' }}>Description:</span> Based on the <a href="/il">IL</a>, <a href="/il">Assembly code</a> is generated.</p>
        <p><span style={{ fontWeight: 'bold' }}>Input:</span> Intermediate code</p>
        <p><span style={{ fontWeight: 'bold' }}>Output:</span> Assembly code</p>
      </div>
    );
  }
}
